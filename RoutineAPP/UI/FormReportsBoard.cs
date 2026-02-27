using RoutineAPP.Application.Services;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Helper;
using RoutineAPP.HelperService;
using RoutineAPP.UI.ViewModel;
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
using static RoutineAPP.Helper.ReportHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormReportsBoard : Form
    {
        private readonly IReportService _reportService;
        private readonly IMonthService _monthService;
        private readonly ICategoryService _categoryService;
        private readonly IDailyRoutineService _dailyRoutineService;

        private List<GetAllMonthsViewModel> _getAllMonthsVM;
        private List<ReportDetailsViewModel> _reportDetailsVM;
        private List<ReportDetailsViewModel> _yearlyReportDetailsVM;

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

        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;

        private void loadMonthlyReports()
        {
            var domainList = _reportService.GetAllMonths();

            _getAllMonthsVM = domainList
                .Select(x => new GetAllMonthsViewModel
                {
                    MonthID = x.MonthID,
                    Month = x.Month,
                    Year = x.Year,
                })
                .ToList();

            dataGridViewMonthly.DataSource = _getAllMonthsVM;
            ConfigureReportDetailsGrid(dataGridViewMonthly, ReportGridType.GetAllMonths);
        }

        private void loadYearlyReports(int year)
        {
            var domainList = _reportService.GetReportDetailsByYear(year);

            _yearlyReportDetailsVM = domainList
                .Select(x => new ReportDetailsViewModel
                {
                    ReportID = x.ReportID,
                    CategoryID = x.CategoryID,
                    Category = x.Category,
                    TotalTimeUsed = x.TotalTimeUsed,
                    PercentageOfUsedTime = x.PercentageOfUsedTime,
                    CompletePercentage = x.CompletePercentage,
                })
                .ToList();

            dataGridViewAnually.DataSource = _yearlyReportDetailsVM;
            ConfigureReportDetailsGrid(dataGridViewAnually, ReportGridType.ReportDetails);
        }

        private void loadOverallReports()
        {
            var domainList = _reportService.GetOverallReportDetails();

            _reportDetailsVM = domainList
                .Select(x => new ReportDetailsViewModel
                {
                    ReportID = x.ReportID,
                    CategoryID = x.CategoryID,
                    Category = x.Category,
                    TotalTimeUsed = x.TotalTimeUsed,
                    PercentageOfUsedTime = x.PercentageOfUsedTime,
                    CompletePercentage = x.CompletePercentage,
                })
                .ToList();

            dataGridViewTotal.DataSource = _reportDetailsVM;
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
            List<GetAllMonthsViewModel> filtered = new List<GetAllMonthsViewModel>();

            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                filtered = _getAllMonthsVM.Where(x => x.MonthID == searchedMonth).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                filtered = _getAllMonthsVM.Where(x => x.Year == searchedYear).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                filtered = _getAllMonthsVM.Where(x => x.Year == searchedYear && x.MonthID == searchedMonth).ToList();
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
           
            var selected = GeneralHelper.GetSelected<GetAllMonthsViewModel>(dataGridViewMonthly);

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
            int? searchedCategory = null;
            int? searchedYear = null;

            if (cmbYearAnually.SelectedIndex != -1)
            {
                searchedYear = Convert.ToInt32(cmbYearAnually.SelectedValue);
            }

            if (cmbCategoryAnually.SelectedIndex != -1)
            {
                searchedCategory = Convert.ToInt32(cmbCategoryAnually.SelectedValue);
            }

            if (searchedYear == null && searchedCategory == null)
            {
                MessageBox.Show("Please select at least a year or category");
                return;
            }

            if (searchedYear != null)
            {
                loadYearlyReports(searchedYear.Value);
            }

            var result = _yearlyReportDetailsVM;

            if (searchedCategory != null)
            {
                result = result
                    .Where(x => x.CategoryID == searchedCategory.Value)
                    .ToList();
            }

            dataGridViewAnually.DataSource = result;

            RefreshCounts();
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

        private void iconBtnClearTotal_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearchTotal_Click(object sender, EventArgs e)
        {
            int searchedCategory;
            List<ReportDetailsViewModel> filtered = new List<ReportDetailsViewModel>();

            if (cmbCategoryTotal.SelectedIndex != -1)
            {               
                searchedCategory = Convert.ToInt32(cmbCategoryTotal.SelectedValue);
                filtered = _reportDetailsVM.Where(x => x.CategoryID == searchedCategory).ToList();
            }            
            else
            {
                MessageBox.Show("Please choose a category");
            }
            dataGridViewTotal.DataSource = filtered;
            RefreshCounts();
        }
    }
}
