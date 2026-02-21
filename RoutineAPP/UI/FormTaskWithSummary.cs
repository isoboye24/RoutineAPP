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
        private readonly IDailyRoutineService _dailyService;
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private int _routineId;
        private int _taskId = 0;
        public FormTaskWithSummary(IDailyRoutineService dailyService, ITaskService taskService, ICategoryService categoryService, int routineId)
        {
            InitializeComponent();
            _dailyService = dailyService;
            _taskService = taskService;
            _categoryService = categoryService;
            _routineId = routineId;
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
                if (_taskId == 0)
                {
                    if (txtTimeSpent.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter time spent in minutes");
                        return;
                    }
                    else if (cmbCategory.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select category");
                        return;
                    }
                    else
                    {
                        int timeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim());
                        _taskService.Create(_routineId, cmbCategory.SelectedIndex, timeSpent, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, txtSummary.Text.Trim());
                    }
                }
                else
                    {
                    if (txtTimeSpent.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter summary");
                        return;
                    }
                    else if (cmbCategory.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select category");
                        return;
                    }
                    else
                    {
                        int timeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim()) + Convert.ToInt32(txtAdditionalTime.Text.Trim());
                        _taskService.Update(_taskId, _routineId, cmbCategory.SelectedIndex, timeSpent, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, txtSummary.Text.Trim());
                    }
                }
                

                MessageBox.Show("Operation successful");
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
