using RoutineAPP.BLL;
using RoutineAPP.DAL.DTO;
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
        public FormTaskWithSummary()
        {
            InitializeComponent();
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
        public bool isUpdate = false;
        public DailyTaskDetailDTO detailDailyRoutine = new DailyTaskDetailDTO();
        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        public TaskDetailDTO detail = new TaskDetailDTO();
        private void FormTaskWithSummary_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtTimeSpent.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            txtAdditionalTime.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            txtSummary.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
            txtAdditionalTime.Hide();
            label1.Hide();
            label5.Hide(); 
            
            if (isUpdate)
            {
                txtAdditionalTime.Visible = true;
                label1.Visible = true;
                label5.Visible = true;
                txtSummary.Text = detail.Summary;
                txtTimeSpent.Text = detail.TimeSpent.ToString();
                cmbCategory.SelectedValue = detail.CategoryID;
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTimeSpent.Text.Trim() == "")
            {
                MessageBox.Show("Enter time spent");
            }
            else if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category is empty");
            }
            else if (!isUpdate)
            {
                int checkTask = bll.CheckTask(Convert.ToInt32(cmbCategory.SelectedValue), detailDailyRoutine.DailyTaskID);
                if (checkTask > 0)
                {
                    MessageBox.Show("This task already exists");
                }
                else
                {
                    TaskDetailDTO task = new TaskDetailDTO();
                    task.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                    task.TimeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim());
                    task.Day = detailDailyRoutine.Day;
                    task.MonthID = detailDailyRoutine.MonthID;
                    task.Year = detailDailyRoutine.Year;
                    task.DailyRoutineID = detailDailyRoutine.DailyTaskID;
                    task.Summary = txtSummary.Text;
                    if (bll.Insert(task))
                    {
                        MessageBox.Show("Task was added successfully");
                        txtTimeSpent.Clear();
                        txtSummary.Clear();
                        cmbCategory.SelectedIndex = -1;
                    }
                } 
            }
            else if (isUpdate)
            {
                if (detail.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue) && detail.TimeSpent == Convert.ToInt32(txtTimeSpent.Text.Trim()) && txtAdditionalTime.Text.Trim() == "" && detail.Summary == txtSummary.Text.Trim())
                {
                    MessageBox.Show("There is no change");
                }
                else
                {
                    detail.Day = detail.Day;
                    detail.MonthID = detail.MonthID;
                    detail.Year = detail.Year;
                    if (txtAdditionalTime.Text.Trim() == "")
                    {
                        detail.TimeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim()) + 0;
                    }
                    else
                    {
                        detail.TimeSpent = Convert.ToInt32(txtTimeSpent.Text.Trim()) + Convert.ToInt32(txtAdditionalTime.Text.Trim());
                    }
                    detail.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                    detail.DailyRoutineID = detail.DailyRoutineID;
                    detail.Summary = txtSummary.Text;
                    if (bll.Update(detail))
                    {
                        MessageBox.Show("Task was updated successfully");
                        this.Close();
                    }
                }
            }
        }

        private void FormTaskWithSummary_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
