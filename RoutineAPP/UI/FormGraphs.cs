using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RoutineAPP.AllForms
{
    public partial class FormGraphs : Form
    {
        private readonly IGraphService _graphService;
        private readonly IDailyRoutineService _dailyRoutineService;
        private readonly IMonthService _monthService;
        private readonly ICategoryService _categoryService;

        private int currYear = DateTime.Today.Year;
        private int currMonth = DateTime.Today.Month;
        private string defaultCategory = "Programming";
        private int defaultCategoryId = 2;

        public FormGraphs(IGraphService graphService, IDailyRoutineService dailyRoutineService, IMonthService monthService, ICategoryService categoryService)
        {
            InitializeComponent();
            _graphService = graphService;
            _dailyRoutineService = dailyRoutineService;
            _monthService = monthService;
            _categoryService = categoryService;
        }


        private void btnClearAllCategories_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void ClearFilters()
        {
            labelGraphTitleAnnualReport.Text = "All Categories " + DateTime.Today.Year;
            cmbAnnualYear.SelectedIndex = -1;

            labelTitleSingleCategory.Text = "Programming " + DateTime.Today.Year;
            cmbCategorySingleCat.SelectedIndex = -1;
            cmbYearSingleCategory.SelectedIndex = -1;

            allCatMonthlyReport.Text = GeneralHelper.ConventIntToMonth(DateTime.Today.Month) + " " + DateTime.Today.Year + " Reports";
            cmbMonthMonthly.SelectedIndex = -1;
            cmbYearMonthly.SelectedIndex = -1;

            loadAllCategoriesAnnualGraph(currYear);

            loadSingleCategoryGraph(currYear, defaultCategoryId, defaultCategory);

            loadMonthlyCategoriesGraph(currMonth, currYear);
        }

        private void resizeControls()
        {
            GeneralHelper.ApplyRegularFont(12, cmbMonthMonthly, cmbCategorySingleCat, cmbAnnualYear, cmbYearSingleCategory, cmbYearMonthly);
            GeneralHelper.ApplyBoldFont(12, iconBtnClear, iconBtnSearch, label1, label4, label5, label6, labelTitleSingleCategory, labelGraphTitleAnnualReport, allCatMonthlyReport);
        }        

        private void loadMonthlyCategoriesGraph(int month, int year)
        {
            var data = _graphService.GetMonthlyCategoriesReport(month, year);
            string monthName = GeneralHelper.ConventIntToMonth(month);


            chartAllCategoryMonthly.Series.Clear();

            chartAllCategoryMonthly.DataSource = data;

            chartAllCategoryMonthly.Series.Add("Hours");
            chartAllCategoryMonthly.Series["Hours"].ChartType = SeriesChartType.Column;
            chartAllCategoryMonthly.Series["Hours"].XValueMember = "CategoryName";
            chartAllCategoryMonthly.Series["Hours"].YValueMembers = "TotalHours";
            chartAllCategoryMonthly.Series["Hours"].IsValueShownAsLabel = true;

            chartAllCategoryMonthly.DataBind();

            chartAllCategoryMonthly.Titles.Clear();
            chartAllCategoryMonthly.Titles.Add($"{monthName} {year} Report");
        }

        private void loadSingleCategoryGraph(int year, int categoryId, string categoryName)
        {
            var data = _graphService.GetSingleCategoryReport(year, categoryId);

            chartSingleCategory.Series.Clear();

            chartSingleCategory.DataSource = data;

            chartSingleCategory.Series.Add("Hours");
            chartSingleCategory.Series["Hours"].ChartType = SeriesChartType.Column;
            chartSingleCategory.Series["Hours"].XValueMember = "Month";
            chartSingleCategory.Series["Hours"].YValueMembers = "TotalHours";
            chartSingleCategory.Series["Hours"].IsValueShownAsLabel = true;

            chartSingleCategory.DataBind();

            chartSingleCategory.Titles.Clear();
            chartSingleCategory.Titles.Add($"{year} {categoryName} Report");

        }

        private void loadAllCategoriesAnnualGraph(int year)
        {
            var data = _graphService.GetAllCategoriesAnnualReport(year);

            chartAnnualReport.Series.Clear();

            chartAnnualReport.DataSource = data;

            chartAnnualReport.Series.Add("Hours");
            chartAnnualReport.Series["Hours"].ChartType = SeriesChartType.Column;
            chartAnnualReport.Series["Hours"].XValueMember = "CategoryName";
            chartAnnualReport.Series["Hours"].YValueMembers = "TotalHours";
            chartAnnualReport.Series["Hours"].IsValueShownAsLabel = true;

            chartAnnualReport.DataBind();

            chartAnnualReport.Titles.Clear();
            chartAnnualReport.Titles.Add($"{year} Annual Report");
        }

        private void loadCombos()
        {
            cmbMonthMonthly.DataSource = _monthService.GetAll();
            GeneralHelper.ComboBoxProps(cmbMonthMonthly, "MonthName", "MonthID");

            cmbCategorySingleCat.DataSource = _categoryService.GetAll();
            GeneralHelper.ComboBoxProps(cmbCategorySingleCat, "CategoryName", "CategoryID");

            cmbAnnualYear.DataSource = _dailyRoutineService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbAnnualYear, "Year", "YearID");

            cmbYearSingleCategory.DataSource = _dailyRoutineService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYearSingleCategory, "Year", "YearID");

            cmbYearMonthly.DataSource = _dailyRoutineService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYearMonthly, "Year", "YearID");
        }


        private void FormGraphs_Load(object sender, EventArgs e)
        {
            resizeControls();

            loadCombos();

            loadAllCategoriesAnnualGraph(currYear);

            loadSingleCategoryGraph(currYear, defaultCategoryId, defaultCategory);

            loadMonthlyCategoriesGraph(currMonth, currYear);
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            if (cmbAnnualYear.SelectedIndex == -1)
            {
                MessageBox.Show("Select a year.");
                return;
            }

            int year = Convert.ToInt32(cmbAnnualYear.SelectedValue);

            var data = _graphService.GetAllCategoriesAnnualReport(year);

            chartAnnualReport.Series[0].Points.Clear();

            foreach (var item in data)
            {
                chartAnnualReport.Series[0].Points.AddXY(
                    item.CategoryName,
                    item.TotalHours);
            }

            labelGraphTitleAnnualReport.Text = year + " Report";
        }

        private void iconBtnClearMonthly_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSingleCatClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconSearchSingleCat_Click(object sender, EventArgs e)
        {
            if (cmbYearSingleCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a year.");
                return;
            }
            else if(cmbCategorySingleCat.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Category.");
                return;
            }
            else
            {
                int year = Convert.ToInt32(cmbYearSingleCategory.SelectedValue);
                int category = Convert.ToInt32(cmbCategorySingleCat.SelectedValue);

                var data = _graphService.GetSingleCategoryReport(year, category);

                chartSingleCategory.Series[0].Points.Clear();

                foreach (var item in data)
                {
                    chartSingleCategory.Series[0].Points.AddXY(
                        item.Month,
                        item.TotalHours);
                }

                labelGraphTitleAnnualReport.Text = year + " Report";
            }
        }

        private void iconBtnSearchMonthly_Click(object sender, EventArgs e)
        {
            
        }
    }
}
