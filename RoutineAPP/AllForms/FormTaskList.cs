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
            cmbCategory.SelectedIndex = -1;
            bll = new TaskBLL();
            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            dataGridView1.DataSource = dto.Tasks;
            RefreshDataCounts();
        }

        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        public DailyTaskDetailDTO detailDailyRoutine = new DailyTaskDetailDTO();
        TaskDetailDTO detail = new TaskDetailDTO();
        private void FormTaskList_Load(object sender, EventArgs e)
        {          
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtSummary.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);            

            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");

            dataGridView1.DataSource = dto.Tasks;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Category";
            dataGridView1.Columns[3].HeaderText = "Time in mins";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].HeaderText = "Time in Hours and mins";
            dataGridView1.Columns[10].Visible = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            labelTitle.Text = detailDailyRoutine.Day + "." + detailDailyRoutine.MonthID + "." + detailDailyRoutine.Year;
            RefreshDataCounts();
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


        private void RefreshDataCounts()
        {
            labelTotalTasks.Text = bll.TotalTasks(detailDailyRoutine.DailyTaskID).ToString();
            decimal totalMinutes = bll.TotalUsedHours(detailDailyRoutine.DailyTaskID);
            int hours = (int)Math.Floor(totalMinutes/60);
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

        private void txtDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new TaskDetailDTO();
            detail.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.CategoryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.CategoryName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.TimeSpent = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            detail.Day = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            detail.MonthName = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            detail.DailyRoutineID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
            detail.TimeInHoursAndMinutes = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            detail.Summary = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            txtSummary.Text = detail.Summary;            
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            if (cmbCategory.SelectedIndex != -1)
            {
                list = list.Where(x => x.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue)).ToList();
            }
            dataGridView1.DataSource = list;
        }

        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            if (detailDailyRoutine.DailyTaskID == 0)
            {
                MessageBox.Show("Please create a routine first");
            }
            else
            {
                FormTaskWithSummary open = new FormTaskWithSummary();
                open.detailDailyRoutine = detailDailyRoutine;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            if (detail.TaskID == 0)
            {
                MessageBox.Show("Please choose a task from the table");
            }
            else
            {
                FormTaskWithSummary open = new FormTaskWithSummary();
                open.detail = detail;
                open.detailDailyRoutine = detailDailyRoutine;
                open.isUpdate = true;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
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

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
