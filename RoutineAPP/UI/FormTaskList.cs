using RoutineAPP.BLL;
using RoutineAPP.DAL.DTO;
using RoutineAPP.HelperService;
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

        private void ApplyFontStyles()
        {
            GeneralHelperService.ApplyBoldFont12(label3);
            GeneralHelperService.ApplyRegularFont12(txtSummary, cmbCategory);
        }

        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        public DailyTaskDetailDTO detailDailyRoutine = new DailyTaskDetailDTO();
        TaskDetailDTO detail = new TaskDetailDTO();
        private void FormTaskList_Load(object sender, EventArgs e)
        {
            ApplyFontStyles();          

            dto = bll.Select(detailDailyRoutine.DailyTaskID);
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");

            LoadDataGridView.loadTasks(dataGridView1, dto);

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Transparent;
            dataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.EnableHeadersVisualStyles = false;

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
            labelTotalTasks.Text = "Total Task" + (dataGridView1.RowCount > 1 ? "s " : " ") + dataGridView1.RowCount.ToString();
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
            detail = GeneralHelperService.MapFromGrid<TaskDetailDTO>(dataGridView1, e.RowIndex);

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
