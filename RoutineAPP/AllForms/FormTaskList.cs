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
    public partial class FormTaskList : Form
    {
        public FormTaskList()
        {
            InitializeComponent();
        }
        private void ClearFilters()
        {
            txtDay.Clear();
            txtYear.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbMonth.SelectedIndex = -1;
            bll = new TaskBLL();
            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            dataGridView1.DataSource = dto.Tasks;
            RefreshDataCounts();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (detailDailyRoutine.DailyTaskID == 0)
            {
                MessageBox.Show("Please create a routine first");
            }
            else
            {
                FormTask open = new FormTask();
                open.detailDailyRoutine = detailDailyRoutine;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }
        
        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        public DailyTaskDetailDTO detailDailyRoutine = new DailyTaskDetailDTO();
        private void FormTaskList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);            
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtDay.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            txtYear.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            btnAdd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnUpdate.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");

            dataGridView1.DataSource = dto.Tasks;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Category";
            dataGridView1.Columns[3].HeaderText = "Time Spent in mins";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            labelTitle.Text = detailDailyRoutine.Day + "." + detailDailyRoutine.MonthID + "." + detailDailyRoutine.Year;
            RefreshDataCounts();
        }
        TaskDetailDTO detail = new TaskDetailDTO();

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            list = list.Where(x => x.Day.ToString().Contains(txtDay.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            list = list.Where(x => x.Year.ToString().Contains(txtYear.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
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

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (detail.TaskID == 0)
            {
                MessageBox.Show("Please choose a task from the table");
            }
            else
            {
                FormTask open = new FormTask();
                open.detail = detail;
                open.detailDailyRoutine = detailDailyRoutine;
                open.isUpdate = true;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (detail.TaskID == 0)
            {
                MessageBox.Show("Please choose a task from the table");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Waring!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (bll.Delete(detail))
                    {
                        MessageBox.Show("Task was deleted successfully");
                        ClearFilters();
                    }
                }
            }
        }
        private void RefreshDataCounts()
        {
            labelTotalTasks.Text = bll.TotalTasks(detailDailyRoutine.DailyTaskID).ToString();
            decimal totalMinutes = bll.TotalUsedHours(detailDailyRoutine.DailyTaskID);
            int hours = Convert.ToInt32(totalMinutes/60);
            int minutes = Convert.ToInt32(totalMinutes % 60);
            if (hours < 1)
            {
                labelTotalTimeUsed.Text = minutes + " min" + (minutes > 1 ? "s" : "");
            }
            else
            {
                labelTotalTimeUsed.Text = hours + " hr" + (hours > 1? "s ":" ") + minutes + " min" + (minutes > 1 ? "s" : "");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_RowEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            detail = new TaskDetailDTO();
            detail.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.CategoryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.CategoryName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.TimeSpent = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            detail.Day = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            detail.MonthName = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            detail.DailyRoutineID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            if (cmbCategory.SelectedIndex != -1)
            {
                list = list.Where(x => x.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue)).ToList();
            }
            if (cmbMonth.SelectedIndex != -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
