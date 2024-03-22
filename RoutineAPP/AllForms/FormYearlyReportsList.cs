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
    public partial class FormYearlyReportsList : Form
    {
        public FormYearlyReportsList()
        {
            InitializeComponent();
        }
        ReportsBLL bll = new ReportsBLL();
        ReportDTO dto = new ReportDTO();
        private void FormTotalReportsList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            cmbCategory.Font = new Font("Segoe UI", 14, FontStyle.Regular);

            dto = bll.Select();
            dataGridViewYears.DataSource = dto.YearlyRoutineReports;
            dataGridViewYears.Columns[0].Visible = false;
            dataGridViewYears.Columns[1].HeaderText = "Year";
            foreach (DataGridViewColumn column in dataGridViewYears.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            dto = bll.SelectYearlyReports(detail.Year);
            dataGridViewCategories.DataSource = dto.YearlyReports;
            dataGridViewCategories.Columns[0].Visible = false;
            dataGridViewCategories.Columns[1].Visible = false;
            dataGridViewCategories.Columns[2].HeaderText = "Category";
            dataGridViewCategories.Columns[3].HeaderText = "Total Time Used";
            dataGridViewCategories.Columns[4].HeaderText = "Percentage of used time";
            foreach (DataGridViewColumn column in dataGridViewCategories.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
        }
        YearlyDetailDTO detail = new YearlyDetailDTO();
        private void dataGridViewYears_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new YearlyDetailDTO();
            detail.YearlyReportID = Convert.ToInt32(dataGridViewYears.Rows[e.RowIndex].Cells[0].Value);
            detail.Year = Convert.ToInt32(dataGridViewYears.Rows[e.RowIndex].Cells[1].Value);
        }
    }
}
