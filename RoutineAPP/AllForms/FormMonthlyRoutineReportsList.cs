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
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtDay.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            txtYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnView.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            dataGridView1.DataSource = dto.MonthlyRoutineReports;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Month";
            dataGridView1.Columns[3].HeaderText = "Year";

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
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
    }
}
