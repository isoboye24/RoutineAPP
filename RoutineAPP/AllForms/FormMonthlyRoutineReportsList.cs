using RoutineAPP.BLL;
using RoutineAPP.DAL.DAO;
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
    public partial class FormMonthlyRoutineReportsList : Form
    {
        public FormMonthlyRoutineReportsList()
        {
            InitializeComponent();
        }
        ReportsBLL bll = new ReportsBLL();
        ReportDTO dto = new ReportDTO();
        
        private void FormMonthlyReportsList_Load(object sender, EventArgs e)
        {
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnView.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");

            dataGridView1.DataSource = dto.MonthlyRoutineReports;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Month";
            dataGridView1.Columns[3].HeaderText = "Year";

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            labelTotal.Text = bll.SelectTotalMonths().ToString();
        }
        MonthlyRoutinesDetailDTO detail = new MonthlyRoutinesDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new MonthlyRoutinesDetailDTO();
            detail.MonthlyReportID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.MonthName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
        }

        private void ClearFilters()
        {
            txtYear.Clear();
            cmbMonth.SelectedIndex = -1;
            bll = new ReportsBLL();
            dto = bll.Select();
            dataGridView1.DataSource = dto.MonthlyRoutineReports;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (detail.MonthlyReportID == 0)
            {
                MessageBox.Show("Please choose a report from the table");
            }
            else
            {
                FormMonthlyReports open = new FormMonthlyReports();
                open.routineDetail = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<MonthlyRoutinesDetailDTO> list = dto.MonthlyRoutineReports;
            if (cmbMonth.SelectedIndex != -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && txtYear.Text.Trim() != "")
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue) && x.Year.ToString().Contains(txtYear.Text.Trim())).ToList();
            }
            dataGridView1.DataSource = list;
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            List<MonthlyRoutinesDetailDTO> list = dto.MonthlyRoutineReports;
            list = list.Where(x => x.Year.ToString().Contains(txtYear.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }
    }
}
