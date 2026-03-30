using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Core.Entities;
using RoutineAPP.Helper;
using System;
using System.Windows.Forms;

namespace RoutineAPP.AllForms
{
    public partial class FormTask : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private int _routineId;
        private DateTime _routineDate;
        private int _taskId;
        private int _prevTimeSpent;
        private int _prevCategoryId;
        private bool _isUpdate = false;

        private DailyRoutineDTO _dailyRoutineDTO;
        private TaskDTO _taskDTO;

        public FormTask(ITaskService taskService, ICategoryService categoryService)
        {
            InitializeComponent();
            _taskService = taskService;
            _categoryService = categoryService;
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

        private void resizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, label2, label3, label5, iconBtnClose, iconBtnSave);
            GeneralHelper.ApplyRegularFont(12, txtTimeSpent, txtAdditionalTime, txtSummary, cmbCategory);
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadForAddTask(DailyRoutineDTO dailyRoutineDTO)
        {
            _dailyRoutineDTO = dailyRoutineDTO;
        }

        public void LoadForEdit(TaskDTO taskDTO, bool isUpdate)
        {
            _taskDTO = taskDTO;
            _isUpdate = isUpdate;           
        }

        private void loadCombos()
        {
            cmbCategory.DataSource = _categoryService.GetAll();
            GeneralHelper.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
        }

        private void FormTaskWithSummary_Load(object sender, EventArgs e)
        {
            resizeControls();

            if (!_isUpdate)
            {
                labelTitle.Text = "Add Task";
                txtAdditionalTime.Hide();
                label1.Hide();
                label5.Hide();

                loadCombos();

                _taskId = _taskDTO.Id;
                _routineId = _taskDTO.DailyRoutineId;
                cmbCategory.SelectedValue = _taskDTO.CategoryId;
                _prevCategoryId = _taskDTO.CategoryId;
                txtSummary.Text = _taskDTO.Summary;
                txtTimeSpent.Text = _taskDTO.TimeSpent.ToString();
                _prevTimeSpent = _taskDTO.TimeSpent;
            }
            else
            {
                labelTitle.Text = "Edit Task";                
            }

        }

        private void FormTaskWithSummary_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int time = Convert.ToInt32(txtTimeSpent.Text.Trim());
                int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);
                string summary = txtSummary.Text.Trim();

                if (_taskDTO.Id == 0)
                {
                    var task = new Task(_dailyRoutineDTO.Id, categoryId,time, _dailyRoutineDTO.Day, _dailyRoutineDTO.MonthID, _dailyRoutineDTO.Year, summary);
                    _taskService.Create(task);
                    MessageBox.Show("Task created successfully!");
                }
                else
                {
                    time += Convert.ToInt32(txtAdditionalTime.Text.Trim());
                    var task = new Task(_dailyRoutineDTO.Id, categoryId, time, _dailyRoutineDTO.Day, _dailyRoutineDTO.MonthID, _dailyRoutineDTO.Year, summary);
                    task.SetId(_taskDTO.Id);
                    _taskService.Update(task);
                    MessageBox.Show("Task updated successfully!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTimeSpent_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }

        private void txtAdditionalTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }
    }
}
