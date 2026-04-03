using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static RoutineAPP.Helper.ReportHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormReportsBoard : Form
    {
        private readonly IReportService _reportService;
        private readonly IMonthService _monthService;
        private readonly ICategoryService _categoryService;
        private readonly IDailyRoutineService _dailyRoutineService;

        private List<GetAllMonthsDTO> _getAllMonthsDTO;
        private List<ReportDTO> _reportTotalDTO;
        private List<ReportDTO> _yearlyReportDTO;
        private List<ReportDTO> _allReportsDTO;

        public FormReportsBoard(IReportService reportService, IMonthService monthService, ICategoryService categoryService, IDailyRoutineService dailyRoutineService)
        {
            InitializeComponent();
            _reportService = reportService;
            _monthService = monthService;
            _categoryService = categoryService;
            _dailyRoutineService = dailyRoutineService;
        }

        private void ResizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, labelYearReportTitle, label2, label3, label4);
            GeneralHelper.ApplyRegularFont(12, labelDateRange, cmbYear, cmbYearAnually, cmbMonth, cmbCategoryAnually, cmbCategoryTotal);
        }

        private void FillCombos()
        {
            cmbMonth.DataSource = _monthService.GetAll();
            GeneralHelper.ComboBoxProps(cmbMonth, "MonthName", "MonthID");

            cmbYear.DataSource = _dailyRoutineService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYear, "Year", "YearID");

            cmbYearAnually.DataSource = _dailyRoutineService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYearAnually, "Year", "YearID");

            cmbCategoryAnually.DataSource = _categoryService.GetAll();
            GeneralHelper.ComboBoxProps(cmbCategoryAnually, "CategoryName", "CategoryID");

            cmbCategoryTotal.DataSource = _categoryService.GetAll();
            GeneralHelper.ComboBoxProps(cmbCategoryTotal, "CategoryName", "CategoryID");
        }

        private int year = DateTime.Now.Year;
        private int month = DateTime.Now.Month;

        private void loadMonthlyReports()
        {
            _getAllMonthsDTO = _reportService.GetAllMonths();
            dataGridViewMonthly.DataSource = _getAllMonthsDTO;
            ConfigureReportDetailsGrid(dataGridViewMonthly, ReportGridType.GetAllMonths);
        }

        private void loadYearlyReports(int year)
        {
            _yearlyReportDTO = _reportService.GetReportDetailsByYear(year);
            _allReportsDTO = _reportService.GetOverallReportDetails();
            dataGridViewAnually.DataSource = _yearlyReportDTO;
            ConfigureReportDetailsGrid(dataGridViewAnually, ReportGridType.ReportDetails);
        }

        private void loadOverallReports()
        {
            _reportTotalDTO = _reportService.GetOverallReportDetails();
            dataGridViewTotal.DataSource = _reportTotalDTO;
            ConfigureReportDetailsGrid(dataGridViewTotal, ReportGridType.ReportDetails);
        }

        private void FormMonthlyReportsList_Load(object sender, EventArgs e)
        {
            ResizeControls();
            FillCombos();

            loadMonthlyReports();

            loadYearlyReports(year);

            loadOverallReports();

            RefreshAnually(year);
            RefreshCounts();

            labelDateRange.Text = _reportService.GetDateRange();

            labelTotalHoursInTotal.Text = _reportService.GetTotalOverallHours();
            labelTotalHoursUsedInTotal.Text = _reportService.GetTotalOverallUsedTime();
            labelTotalHoursUnusedInTotal.Text = _reportService.GetTotalOverallUnusedTime();
        }
        

        private void ClearFilters()
        {
            cmbYear.SelectedIndex = -1;
            cmbMonth.SelectedIndex = -1;
            cmbCategoryAnually.SelectedIndex = -1;
            cmbYearAnually.SelectedIndex = -1;
            cmbCategoryTotal.SelectedIndex = -1;

            loadMonthlyReports();

            loadYearlyReports(year);

            loadOverallReports();

            RefreshCounts();
            RefreshAnually(year);
        }

        private void RefreshCounts()
        {
            labelTotal.Text = dataGridViewMonthly.RowCount + " Month" + (dataGridViewMonthly.RowCount > 0 ? "s" : "");
        }
        
        private void RefreshAnually(int year)
        {
            labelYearReportTitle.Text =  year + " Report";
            labelTotalHoursInYear.Text = "Total hours in " + year + " : " + _reportService.GetTotalHoursInYear(year);
            labelTotalHoursUsedInYear.Text = "Total hours used in " + year + " : " + _reportService.GetTotalUsedTimeInYear(year);
            labelTotalHoursUnusedInYear.Text = "Total hours unused in " + year + " : " + _reportService.GetTotalUnusedTimeInYear(year);
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            int searchedMonth;
            int searchedYear;
            List<GetAllMonthsDTO> filtered = new List<GetAllMonthsDTO>();

            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                filtered = _getAllMonthsDTO.Where(x => x.MonthID == searchedMonth).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                filtered = _getAllMonthsDTO.Where(x => x.Year == searchedYear).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                filtered = _getAllMonthsDTO.Where(x => x.Year == searchedYear && x.MonthID == searchedMonth).ToList();
            }
            else
            {
                MessageBox.Show("Please at least a month or year");
            }
            dataGridViewMonthly.DataSource = filtered;

            RefreshCounts();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
           
            var selected = GeneralHelper.GetSelected<GetAllMonthsDTO>(dataGridViewMonthly);

            if (selected == null)
            {
                MessageBox.Show("Please choose a routine from the table.");
                return;
            }

            var form = new FormMonthlyReports(_reportService, _categoryService);
            form.LoadForView(selected.MonthID, selected.Year);
            form.ShowDialog();
        }

        private void dataGridViewAnually_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
        }

        private void iconBtnSearchAnually_Click(object sender, EventArgs e)
        {
            if (cmbCategoryAnually.SelectedIndex == -1 && cmbYearAnually.SelectedIndex == -1)
            {
                MessageBox.Show("Please select at least a category or year");
                return;
            }
            else
            {
                var query = _allReportsDTO.AsEnumerable();
                int selectedCategory = Convert.ToInt32(cmbCategoryAnually.SelectedValue);
                int selectedYear = Convert.ToInt32(cmbYearAnually.SelectedValue);
                bool hasYear = cmbYearAnually.SelectedIndex > -1;

                if (cmbCategoryAnually.SelectedIndex != -1 && cmbYearAnually.SelectedIndex != -1)
                {
                    query = query.Where(x => x.Year == selectedYear && x.CategoryID == selectedCategory);
                }
                else if (cmbYearAnually.SelectedIndex != -1 && cmbCategoryAnually.SelectedIndex == -1)
                {
                    query = query.Where(x => x.Year == selectedYear);
                }
                else
                {
                    query = query.Where(x => x.CategoryID == selectedCategory);
                }

                var filtered = query.ToList();

                dataGridViewAnually.DataSource = filtered;

                RefreshAnually(hasYear ? selectedYear : DateTime.Today.Year);
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

        private void iconBtnClearTotal_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearchTotal_Click(object sender, EventArgs e)
        {
            if (cmbCategoryTotal.SelectedIndex != -1)
            {
                int searchedCategory = Convert.ToInt32(cmbCategoryTotal.SelectedValue);

                var filtered = _reportTotalDTO.Where(x => x.CategoryID == searchedCategory).ToList();

                dataGridViewTotal.DataSource = filtered;
                RefreshCounts();
            }            
            else
            {
                MessageBox.Show("Please choose a category from the dropdown");
            }            
        }

        private void dataGridViewMonthly_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
        }

        private void dataGridViewAnually_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            GeneralHelper.ApplyRankingColors((DataGridView)sender, e);
        }

        private void dataGridViewTotal_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            GeneralHelper.ApplyRankingColors((DataGridView)sender, e);
        }
    }
}
