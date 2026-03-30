using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static RoutineAPP.Helper.TaskHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormTaskList : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;

        private int _routineId;
        private DateTime _routineDate;
        private List<TaskDTO> _taskVM;

        public FormTaskList(ITaskService taskService, ICategoryService categoryService, IReportService reportService)
        {
            InitializeComponent();
            _taskService = taskService;
            _categoryService = categoryService;
            _reportService = reportService;
        }

        public void LoadForView(int id, DateTime date)
        {
            _routineId = id;
            _routineDate = date;
        }


        private void loadTasks()
        {
            dataGridView1.DataSource = _taskService.GetTasksByDay(_routineId);
            ConfigureTaskGrid(dataGridView1, TaskGridType.Basic);
        }

        private void ClearFilters()
        {
            cmbCategory.SelectedIndex = -1;
            
            loadTasks();
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

        private void FormTaskList_Load(object sender, EventArgs e)
        {
            ApplyFontStyles();
            LoadCombo();

            loadTasks();

            labelTitle.Text = _routineDate.ToString("dd.MM.yyyy");

            RefreshDataCounts();
        }

        private void RefreshDataCounts()
        {
            labelTotalTimeUsed.Text = "Used Time : " + _reportService.GetTotalUsedTimeInDay(_routineId);
            labelTotalUnusedTime.Text = "Unused Time : " + _reportService.GetTotalUnusedTimeInDay(_routineId);
            labelTotalTasks.Text = dataGridView1.RowCount + " Task" + (dataGridView1.RowCount > 1 ? "s" : "");
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
            var selected = GeneralHelper.GetSelected<DailyRoutineDTO>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormTask(_taskService, _categoryService);
            form.LoadForAddTask(selected);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<TaskDTO>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormTask(_taskService, _categoryService);
            form.LoadForEdit(selected, true);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<TaskDTO>(dataGridView1);
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
