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
        public MonthlyRoutinesDetailDTO routineDetail = new MonthlyRoutinesDetailDTO();
        private void FormTotalReportsList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            txtYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);

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
            dataGridViewCategories.Columns[4].HeaderText = "Percentage in 2 dp";
            dataGridViewCategories.Columns[5].HeaderText = "Complete value in %";
            dataGridViewCategories.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewCategories.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }

            labelTotalYealyHours.Text = "Total hours in " + detail.Year + " :";
            label2.Text = bll.SelectTotalHoursInYear(detail.Year).ToString();
            labelTotalHoursUsed.Text = bll.SelectTotalHoursUsedInAYear(detail.Year).ToString();
            this.Text = detail.Year + " Report";
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
        }
        YearDetailDTO detail = new YearDetailDTO();
        private void dataGridViewYears_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new YearDetailDTO();
            detail.YearID = Convert.ToInt32(dataGridViewYears.Rows[e.RowIndex].Cells[0].Value);
            detail.Year = Convert.ToInt32(dataGridViewYears.Rows[e.RowIndex].Cells[1].Value);
        }

        private void dataGridViewCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    if (cellValue * 100 <= 5)
                    {
                        DataGridViewRow row = dataGridViewCategories.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Red;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else if (cellValue * 100 > 5 && cellValue * 100 <= 10)
                    {
                        DataGridViewRow row = dataGridViewCategories.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Yellow;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else if (cellValue * 100 > 10 && cellValue * 100 <= 25)
                    {
                        DataGridViewRow row = dataGridViewCategories.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGreen;
                            cell.Style.ForeColor = Color.White;
                        }
                    }
                    else if (cellValue * 100 > 25)
                    {
                        DataGridViewRow row = dataGridViewCategories.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGoldenrod;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        DataGridViewRow row = dataGridViewCategories.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = dataGridViewCategories.DefaultCellStyle.BackColor;
                            cell.Style.ForeColor = dataGridViewCategories.DefaultCellStyle.ForeColor;
                        }
                    }
                }
            }
        }

        private void ClearFilters()
        {
            txtYear.Clear();
            cmbCategory.SelectedIndex = -1;
            bll = new ReportsBLL();
            dto = bll.SelectYearlyReports(detail.Year);
            dataGridViewCategories.DataSource = dto.YearlyReports;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<ReportsDetailDTO> list = dto.YearlyReports;
            if (cmbCategory.SelectedIndex != -1)
            {
                list = list.Where(x => x.Year == detail.Year && x.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue)).ToList();
            }
            dataGridViewCategories.DataSource = list;
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            List<ReportsDetailDTO> list = dto.YearlyReports;
            list = list.Where(x => x.Year.ToString().Contains(txtYear.Text)).ToList();
            dataGridViewCategories.DataSource = list;
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }
    }
}
