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
    public partial class FormTask : Form
    {
        public FormTask()
        {
            InitializeComponent();
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        public TaskDetailDTO detail = new TaskDetailDTO();
        private void FormTask_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtTimeSpent.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
            if (isUpdate)
            {
                txtTimeSpent.Text = detail.TimeSpent.ToString();
                cmbCategory.SelectedValue = detail.CategoryID;
            }

        }
        public bool isUpdate = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTimeSpent.Text.Trim() == "")
            {
                MessageBox.Show("Time spent is empty");
            }
            else if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category is empty");
            }
            else
            {
                if (!isUpdate)
                {
                    TaskDetailDTO task = new TaskDetailDTO();
                    task.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                    task.TimeSpent = Convert.ToDecimal(txtTimeSpent.Text.Trim());
                    task.Day = DateTime.Today.Day;
                    task.MonthID = DateTime.Today.Month;
                    task.Year = DateTime.Today.Year;
                    if (bll.Insert(task))
                    {
                        MessageBox.Show("Task was added successfully");
                        txtTimeSpent.Clear();
                        cmbCategory.SelectedIndex = -1;
                    }
                } 
                else if (isUpdate)
                {
                    if (detail.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue) && detail.TimeSpent == Convert.ToDecimal(txtTimeSpent.Text.Trim()))
                    {
                        MessageBox.Show("There is no change");
                    }
                    else
                    {
                        detail.Day = detail.Day;
                        detail.MonthID = detail.MonthID;
                        detail.Year = detail.Year;
                        detail.TimeSpent = Convert.ToDecimal(txtTimeSpent.Text.Trim());
                        detail.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                        if (bll.Update(detail))
                        {
                            MessageBox.Show("Task was updated successfully");
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
