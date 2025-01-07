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
    public partial class FormSummaryList : Form
    {
        public FormSummaryList()
        {
            InitializeComponent();
        }
        DailyTaskBLL bll = new DailyTaskBLL();
        DailyTaskDTO dto = new DailyTaskDTO();
        DailyTaskDetailDTO detail = new DailyTaskDetailDTO();

        private void FontSizes()
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelTotalComments.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtDay.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 12, FontStyle.Regular);
        }
        private void FormCommentList_Load(object sender, EventArgs e)
        {
            FontSizes();

            dto = bll.SelectSummaries();
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");
            cmbYear.DataSource = dto.Years;
            General.ComboBoxProps(cmbYear, "Year", "YearID");

            dataGridView1.DataSource = dto.Summaries;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Comment";
            dataGridView1.Columns[3].HeaderText = "Day";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].HeaderText = "Month";
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].HeaderText = "Year";
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            List<DailyTaskDetailDTO> list = dto.Summaries;
            list = list.Where(item => item.Summary != "" && item.Summary != null).ToList();
            dataGridView1.DataSource = list;

            RefreshDataCounts();
        }
        private void RefreshDataCounts()
        {
            labelTotalComments.Text = dataGridView1.RowCount.ToString();
        }
        private void ClearFilters()
        {
            txtComment.Clear();
            txtDay.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            
            dto = bll.SelectSummaries();
            dataGridView1.DataSource = dto.Summaries;
            RefreshDataCounts();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new DailyTaskDetailDTO();
            detail.DailyTaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.RoutineDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.Summary = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Day = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.MonthName = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {
            List<DailyTaskDetailDTO> list = dto.Summaries;
            list = list.Where(x => x.Summary.Contains(txtComment.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
            RefreshDataCounts();
        }

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            List<DailyTaskDetailDTO> list = dto.Summaries;
            list = list.Where(x => x.Day.ToString().Contains(txtDay.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
            RefreshDataCounts();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
            if (detail.DailyTaskID == 0)
            {
                MessageBox.Show("Please select a comment from the table");
            }
            else
            {
                FormDailyRoutine open = new FormDailyRoutine();
                open.isSummaryList = true;
                open.detail = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            List<DailyTaskDetailDTO> list = dto.Summaries;
            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue) && x.Year == Convert.ToInt32(cmbYear.Text)).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                list = list.Where(x => x.Year == Convert.ToInt32(cmbYear.Text)).ToList();
            }
            dataGridView1.DataSource = list;
            RefreshDataCounts();
        }
    }
}
