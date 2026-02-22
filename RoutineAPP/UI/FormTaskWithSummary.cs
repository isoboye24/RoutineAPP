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

namespace RoutineAPP.AllForms
{
    public partial class FormTaskWithSummary : Form
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private int _routineId;
        private DateTime _routineDate;
        private int _taskId;
        public FormTaskWithSummary(ITaskService taskService, ICategoryService categoryService)
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
            GeneralHelperService.ApplyBoldFont(12, label1, label2, label3, label5, iconBtnClose, iconBtnSave);
            GeneralHelperService.ApplyRegularFont(12, txtTimeSpent, txtAdditionalTime, txtSummary, cmbCategory);
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void FormTaskWithSummary_Load(object sender, EventArgs e)
        {
            resizeControls();

            cmbCategory.DataSource = _categoryService.GetAll();
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");

            txtAdditionalTime.Hide();
            label1.Hide();
            label5.Hide(); 
            
        }
        public void LoadForAddTask(int id, DateTime date)
        {
            _routineId = id;
            _routineDate = date;
        }

        public void LoadForEdit(TaskViewModel vm)
        {
            _taskId = vm.Id;
            _routineId = vm.DailyRoutineId;
            cmbCategory.SelectedValue = vm.CategoryId;
            txtSummary.Text = vm.Summary;
            txtTimeSpent.Text = vm.TimeSpent.ToString();
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
                if (string.IsNullOrWhiteSpace(txtTimeSpent.Text))
                {
                    MessageBox.Show("Please enter time spent in minutes");
                    return;
                }

                if (cmbCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select category");
                    return;
                }

                int timeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim());

                // Use SelectedValue, NOT SelectedIndex
                int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

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
                    task.SetId(_taskId);
                    _taskService.Update(task);
                    MessageBox.Show("Task updated successfully!");
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
            e.Handled = General.isNumber(e);
        }
    }
}
