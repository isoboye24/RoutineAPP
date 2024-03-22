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
    public partial class FormMonthlyReports : Form
    {
        public FormMonthlyReports()
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
        ReportsBLL bll = new ReportsBLL();
        ReportDTO dto = new ReportDTO();
        public MonthlyRoutinesDetailDTO routineDetail = new MonthlyRoutinesDetailDTO();
        private void FormMonthlyReports_Load(object sender, EventArgs e)
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
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.SelectMonthlyReports(routineDetail.MonthID, routineDetail.Year);
            dataGridView1.DataSource = dto.MonthlyReports;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Category";
            dataGridView1.Columns[3].HeaderText = "Total Time Used";
            dataGridView1.Columns[4].HeaderText = "Percentage of used time";
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            labelTotalHours.Text = bll.SelectTotalHoursInMonth(routineDetail.MonthID, routineDetail.Year).ToString();
            labelTotalHoursUsed.Text = bll.SelectTotalHoursUsedInMonth(routineDetail.MonthID, routineDetail.Year);
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[4], System.ComponentModel.ListSortDirection.Descending);
        }
    }
}
