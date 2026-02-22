using RoutineAPP.BLL;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.DAL.DTO;
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
using static RoutineAPP.HelperService.TaskHelperService;

namespace RoutineAPP.AllForms
{
    public partial class FormTaskList : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private int _routineId;
        private DateTime _routineDate;
        private List<TaskViewModel> _taskVM;

        public FormTaskList(ITaskService taskService, ICategoryService categoryService)
        {
            InitializeComponent();
            _taskService = taskService;
            _categoryService = categoryService;
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
            GeneralHelperService.ApplyBoldFont(12, label3);
            GeneralHelperService.ApplyRegularFont(12, txtSummary, cmbCategory);
        }

        private void LoadCombo() {
            cmbCategory.DataSource = _categoryService.GetAll();
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
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
            labelTotalTimeUsed.Text = _taskService.DailyUsedTimeCount(_routineId);
            labelTotalUnusedTime.Text = _taskService.DailyUnusedTimeCount(_routineId);
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
            var selected = General.GetSelected<TaskViewModel>(dataGridView1);
            var form = new FormTaskWithSummary(_taskService, _categoryService);
            form.LoadForAddTask(selected.DailyRoutineId, selected.DailyRoutineDate);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = General.GetSelected<TaskViewModel>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormTaskWithSummary(_taskService, _categoryService);
            form.LoadForEdit(selected);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = General.GetSelected<TaskViewModel>(dataGridView1);
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
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (e.RowIndex == 0)
            {
                row.DefaultCellStyle.BackColor = Color.DarkOrange;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
            else if (e.RowIndex == 1)
            {
                row.DefaultCellStyle.BackColor = Color.YellowGreen;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
            else if (e.RowIndex == 2)
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.IndianRed;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
    }
}
