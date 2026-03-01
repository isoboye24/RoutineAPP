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

        public void LoadForAddTask(int id, DateTime date)
        {
            _routineId = id;
            _routineDate = date;            
        }

        public void LoadForEdit(TaskViewModel vm, bool isUpdate)
        {
            loadCombos();

            _taskId = vm.Id;
            _routineId = vm.DailyRoutineId;
            cmbCategory.SelectedValue = vm.CategoryId;
            _prevCategoryId = vm.CategoryId;
            txtSummary.Text = vm.Summary;
            txtTimeSpent.Text = vm.TimeSpent.ToString();
            _prevTimeSpent = vm.TimeSpent;
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
                if (!int.TryParse(txtTimeSpent.Text.Trim(), out int timeSpent))
                {
                    MessageBox.Show("Please enter time spent in minutes");
                    return;
                }

                if (cmbCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select category");
                    return;
                }

                // Use SelectedValue, NOT SelectedIndex
                int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

                timeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim());
                int additionalTime = string.IsNullOrWhiteSpace(txtAdditionalTime.Text.Trim()) ? 0 : Convert.ToInt32(txtAdditionalTime.Text.Trim());

                if (_isUpdate)
                {
                    timeSpent += additionalTime;                                        
                }                

                var task = new RoutineAPP.Core.Entities.Task(
                    _routineId,
                    categoryId,
                    timeSpent,
                    _routineDate.Day,
                    _routineDate.Month,
                    _routineDate.Year,
                    txtSummary.Text.Trim()
                );

                if (_taskId == 0)
                {
                    _taskService.Create(task);
                    MessageBox.Show("Task created successfully!");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtAdditionalTime.Text.Trim()) && _prevTimeSpent == timeSpent && _prevCategoryId == categoryId)
                    {
                        MessageBox.Show("No changes");
                    }
                    else
                    {
                        task.SetId(_taskId);
                        _taskService.Update(task);
                        MessageBox.Show("Task updated successfully!");
                    }
                }
                
                this.Close();
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
