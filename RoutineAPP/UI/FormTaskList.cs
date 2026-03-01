using Microsoft.Extensions.DependencyInjection;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RoutineAPP.HelperService.TaskHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormTaskList : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;
        private readonly IServiceProvider _serviceProvider;

        private int _routineId;
        private DateTime _routineDate;
        private List<TaskViewModel> _taskVM;

        public FormTaskList(ITaskService taskService, ICategoryService categoryService, IReportService reportService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _taskService = taskService;
            _categoryService = categoryService;
            _reportService = reportService;
            _serviceProvider = serviceProvider;
        }

        public void LoadForView(int id, DateTime date)
        {
            _routineId = id;
            _routineDate = date;
        }


        private void loadTasks()
        {
            var domainList = _taskService.GetTaskDetails(_routineId);

            _taskVM = domainList.ToList();

            _taskVM = domainList
                .Select(x => new TaskViewModel
                {
                    Id = x.Id,
                    DailyRoutineId = x.DailyRoutineId,
                    DailyRoutineDate = x.DailyRoutineDate,
                    CategoryId = x.CategoryId,
                    Category = x.Category,
                    TimeSpent = x.TimeSpent,
                    Day = x.DailyRoutineDate.Day,
                    MonthID = x.DailyRoutineDate.Month,
                    MonthName = GeneralHelper.ConventIntToMonth(x.DailyRoutineDate.Month),
                    Year = x.DailyRoutineDate.Year,
                    Summary = x.Summary,
                    TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.TimeSpent)
                }).OrderByDescending(x => x.TimeSpent).ThenBy(x => x.Category)
                .ToList();

            dataGridView1.DataSource = _taskVM;
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
            List<TaskViewModel> filtered = new List<TaskViewModel>();

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
            var form = _serviceProvider.GetRequiredService<FormTask>();
            form.LoadForAddTask(_routineId, _routineDate);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<TaskViewModel>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = _serviceProvider.GetRequiredService<FormTask>();
            form.LoadForEdit(selected, true);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<TaskViewModel>(dataGridView1);
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
