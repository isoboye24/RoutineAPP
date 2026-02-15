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
using System.Windows.Documents;
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
        int year = DateTime.Now.Year;
        
        private void FormMonthlyReportsList_Load(object sender, EventArgs e)
        {
            labelDateRange.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelYearReportTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            cmbYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYearAnually.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategoryAnually.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategoryTotal.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            dto = bll.Select(year);
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");
            cmbYear.DataSource = dto.Years;
            General.ComboBoxProps(cmbYear, "Year", "YearID");
            cmbYearAnually.DataSource = dto.Years;
            General.ComboBoxProps(cmbYearAnually, "Year", "YearID");

            cmbCategoryAnually.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategoryAnually, "CategoryName", "CategoryID");
            cmbCategoryTotal.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategoryTotal, "CategoryName", "CategoryID");

            dataGridViewMonthly.DataSource = dto.MonthlyRoutineReports;
            dataGridViewMonthly.Columns[0].Visible = false;
            dataGridViewMonthly.Columns[1].Visible = false;
            dataGridViewMonthly.Columns[2].HeaderText = "Month";
            dataGridViewMonthly.Columns[3].HeaderText = "Year";

            foreach (DataGridViewColumn column in dataGridViewMonthly.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }

            dataGridViewAnually.DataSource = dto.YearlyReports;
            dataGridViewAnually.Columns[0].Visible = false;
            dataGridViewAnually.Columns[1].Visible = false;
            dataGridViewAnually.Columns[2].HeaderText = "Category";
            dataGridViewAnually.Columns[3].HeaderText = "Total Time Used";
            dataGridViewAnually.Columns[4].HeaderText = "Percentage in 2 dp";
            dataGridViewAnually.Columns[5].HeaderText = "Complete value in %";
            dataGridViewAnually.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewAnually.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }

            dataGridViewTotal.DataSource = dto.TotalReports;
            dataGridViewTotal.Columns[0].Visible = false;
            dataGridViewTotal.Columns[1].Visible = false;
            dataGridViewTotal.Columns[2].HeaderText = "Category";
            dataGridViewTotal.Columns[3].HeaderText = "Total Time Used";
            dataGridViewTotal.Columns[4].HeaderText = "Percentage in 2 dp";
            dataGridViewTotal.Columns[5].HeaderText = "Complete value in %";
            dataGridViewTotal.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewTotal.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }

            RefreshAnually(year);
            RefreshCounts();

            labelDateRange.Text = bll.SelectReportDateRange().ToString();

            labelTotalHoursInTotal.Text = bll.SelectOverallTotalHours().ToString();
            labelTotalHoursUsedInTotal.Text = bll.SelectOverallTotalUsedHours().ToString();
            labelTotalHoursUnusedInTotal.Text = bll.SelectOverallTotalUnusedHours().ToString();
        }
        MonthlyRoutinesDetailDTO detail = new MonthlyRoutinesDetailDTO();
        
        private void ClearFilters()
        {
            cmbYear.SelectedIndex = -1;
            cmbMonth.SelectedIndex = -1;
            cmbCategoryAnually.SelectedIndex = -1;
            cmbYearAnually.SelectedIndex = -1;
            bll = new ReportsBLL();
            dto = bll.Select(year);
            dataGridViewMonthly.DataSource = dto.MonthlyRoutineReports;
            dataGridViewAnually.DataSource = dto.YearlyReports;
            RefreshCounts();
            RefreshAnually(year);
        }

        private void RefreshCounts()
        {
            labelTotal.Text = dataGridViewMonthly.RowCount.ToString();
        }
        
        private void RefreshAnually(int year)
        {
            labelTotalYealyHours.Text = "Total hours in " + year + " :";
            labelYearReportTitle.Text =  year + " Report";
            labelTotalHoursInYear.Text = bll.SelectTotalHoursInYear(year).ToString();
            labelTotalHoursUsed.Text = bll.SelectTotalHoursUsedInAYear(year).ToString();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            List<MonthlyRoutinesDetailDTO> list = dto.MonthlyRoutineReports;
            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != 1)
            {
                list = list.Where(x => x.Year.ToString() == cmbYear.Text && x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            else if (cmbYear.SelectedIndex != -1)
            {
                list = list.Where(x => x.Year.ToString() == cmbYear.Text).ToList();
            }
            else
            {
                MessageBox.Show("Unknown search");
            }
            dataGridViewMonthly.DataSource = list;
            RefreshCounts();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
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
                ClearFilters();
            }
        }

        private void dataGridViewMonthly_RowEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            detail = new MonthlyRoutinesDetailDTO();
            detail.MonthlyReportID = Convert.ToInt32(dataGridViewMonthly.Rows[e.RowIndex].Cells[0].Value);
            detail.MonthID = Convert.ToInt32(dataGridViewMonthly.Rows[e.RowIndex].Cells[1].Value);
            detail.MonthName = dataGridViewMonthly.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridViewMonthly.Rows[e.RowIndex].Cells[3].Value);
        }

        private void dataGridViewAnually_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    if (cellValue * 100 < 5)
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Red;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    
                    else if (cellValue * 100 >= 5 && cellValue * 100 <= 10)
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Yellow;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else if (cellValue * 100 > 10 && cellValue * 100 <= 25)
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGreen;
                            cell.Style.ForeColor = Color.White;
                        }
                    }
                    else if (cellValue * 100 > 25)
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGoldenrod;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = dataGridViewAnually.DefaultCellStyle.BackColor;
                            cell.Style.ForeColor = dataGridViewAnually.DefaultCellStyle.ForeColor;
                        }
                    }
                }
            }
        }

        private void iconBtnSearchAnually_Click(object sender, EventArgs e)
        {            
            if (cmbCategoryAnually.SelectedIndex != -1 && cmbYearAnually.SelectedIndex != -1)
            {
                dto = bll.Select(Convert.ToInt32(cmbYearAnually.Text));
                List<ReportsDetailDTO> list = dto.YearlyReports;
                list = list.Where(x => x.Year == Convert.ToInt32(cmbYearAnually.Text) && x.CategoryID == Convert.ToInt32(cmbCategoryAnually.SelectedValue)).ToList();
                dataGridViewAnually.DataSource = list;
                RefreshAnually(Convert.ToInt32(cmbYearAnually.Text));
            }
            else if (cmbCategoryAnually.SelectedIndex != -1)
            {
                dto = bll.Select(year);
                List<ReportsDetailDTO> list = dto.YearlyReports;
                list = list.Where(x => x.Year == year && x.CategoryID == Convert.ToInt32(cmbCategoryAnually.SelectedValue)).ToList();
                dataGridViewAnually.DataSource = list;
            }
            else if (cmbYearAnually.SelectedIndex != -1)
            {
                dto = bll.Select(Convert.ToInt32(cmbYearAnually.Text));
                List<ReportsDetailDTO> list = dto.YearlyReports;
                list = list.Where(x => x.Year == Convert.ToInt32(cmbYearAnually.Text)).ToList();
                RefreshAnually(Convert.ToInt32(cmbYearAnually.Text));
                dataGridViewAnually.DataSource = list;
            }
            else
            {
                MessageBox.Show("Unknown search");
            }            
        }

        private void iconBtnClearAnually_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void dataGridViewAnually_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Draw row numbers on the row header
            using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(dataGridViewAnually.RowHeadersDefaultCellStyle.ForeColor))
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

        private void dataGridViewTotal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    if (cellValue * 100 < 5)
                    {
                        DataGridViewRow row = dataGridViewTotal.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Red;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }

                    else if (cellValue * 100 >= 5 && cellValue * 100 <= 10)
                    {
                        DataGridViewRow row = dataGridViewTotal.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Yellow;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else if (cellValue * 100 > 10 && cellValue * 100 <= 25)
                    {
                        DataGridViewRow row = dataGridViewTotal.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGreen;
                            cell.Style.ForeColor = Color.White;
                        }
                    }
                    else if (cellValue * 100 > 25)
                    {
                        DataGridViewRow row = dataGridViewTotal.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGoldenrod;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        DataGridViewRow row = dataGridViewAnually.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = dataGridViewTotal.DefaultCellStyle.BackColor;
                            cell.Style.ForeColor = dataGridViewTotal.DefaultCellStyle.ForeColor;
                        }
                    }
                }
            }
        }

        private void dataGridViewTotal_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Draw row numbers on the row header
            using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(dataGridViewTotal.RowHeadersDefaultCellStyle.ForeColor))
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
    }
}
