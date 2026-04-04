using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static RoutineAPP.Helper.TaskHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormTaskList : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;
        private readonly IGraphService _graphService;

        private List<TaskDTO> _taskVM;
        private DailyRoutineDTO _routineViewDTO;

        public FormTaskList(ITaskService taskService, ICategoryService categoryService, IReportService reportService, IGraphService graphService)
        {
            InitializeComponent();
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));
        }

        public void LoadForView(DailyRoutineDTO routineViewDTO)
        {
            _routineViewDTO = routineViewDTO;
        }

        private void loadTasks()
        {
            dataGridView1.DataSource = _taskService.GetTasksByDay(_routineViewDTO.Id);
            ConfigureTaskGrid(dataGridView1, TaskGridType.Basic);
        }

        private void ClearFilters()
        {
            cmbCategory.SelectedIndex = -1;
            
            loadTasks();
            loadPieChart(_routineViewDTO.Id);
            RefreshDataCounts();
        }

        private void ApplyFontStyles()
        {
            GeneralHelper.ApplyBoldFont(12, label3);
            GeneralHelper.ApplyRegularFont(12, txtSummary, cmbCategory);
        }

        private void LoadCombo() {
            cmbCategory.DataSource = _categoryService.GetAll();
            GeneralHelper.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
        }

        private void RefreshDataCounts()
        {
            labelTotalTimeUsed.Text = ("Used Time : " + _reportService.GetTotalUsedTimeInDay(_routineViewDTO.Id)).ToString();
            labelTotalUnusedTime.Text = ("Unused Time : " + _reportService.GetTotalUnusedTimeInDay(_routineViewDTO.Id)).ToString();
            labelTotalTasks.Text = dataGridView1.RowCount + " Task" + (dataGridView1.RowCount > 1 ? "s" : "").ToString();
        }

        private void loadPieChart(int routineId)
        {
            var data = _graphService.GetDailyReport(routineId);

            dailyPieChart.Series.Clear();

            var series = new Series("Minutes");
            series.ChartType = SeriesChartType.Pie;

            foreach (var item in data)
            {
                series.Points.AddXY(item.CategoryName, item.TotalMinutes);
            }

            series.IsValueShownAsLabel = true;
            series.Label = "#VALX: #PERCENT{P0}";

            series["PieCollectedThreshold"] = "0";
            series["PieCollectedStyle"] = "None";

            dailyPieChart.Series.Add(series);

            dailyPieChart.Titles.Clear();
            dailyPieChart.Titles.Add("Daily Category Distribution");
        }


        private void FormTaskList_Load(object sender, EventArgs e)
        {
            ApplyFontStyles();
            LoadCombo();

            loadTasks();

            loadPieChart(_routineViewDTO.Id);

            labelTitle.Text = _routineViewDTO.RoutineDate.ToString("dd.MM.yyyy");

            RefreshDataCounts();
        }

        private void iconMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void iconMinimize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDTO> filtered = new List<TaskDTO>();

            if (cmbCategory.SelectedIndex != -1)
            {
                int searchedCategory = Convert.ToInt32(cmbCategory.SelectedValue);
                filtered = _taskVM.Where(x => x.CategoryId == searchedCategory).ToList();
            }            
            else
            {
                MessageBox.Show("Please select a category");
            }
            dataGridView1.DataSource = filtered;

            RefreshDataCounts();
        }

        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            var form = new FormTask(_taskService, _categoryService);
            form.LoadForAddTask(_routineViewDTO);
            form.ShowDialog();

            ClearFilters();
        }

        private TaskDTO GetSelectedTask()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            return dataGridView1.CurrentRow.DataBoundItem as TaskDTO;
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedTask();
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormTask(_taskService, _categoryService);
            form.LoadForEdit(selected);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedTask();
            if (selected == null)
            {
                MessageBox.Show("Please select a task.");
                return;
            }

            var result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _taskService.Delete(selected.Id);
                ClearFilters();
            }
        }

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                string rowNumber = (e.RowIndex + 1).ToString();
                e.Graphics.DrawString(
                    rowNumber,
                    font,
                    brush,
                    e.RowBounds.Location.X + 15,
                    e.RowBounds.Location.Y + 4
                );
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            GeneralHelper.ApplyRankingColors((DataGridView)sender, e);
        }

    }
}
