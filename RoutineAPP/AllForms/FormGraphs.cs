using RoutineAPP.BLL;
using RoutineAPP.DAL.DTO;
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
        public FormGraphs()
        {
            InitializeComponent();
        }

        GraphBLL bll = new GraphBLL();
        GraphDTO dto = new GraphDTO();

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

            allCatMonthlyReport.Text = General.ConventIntToMonth(DateTime.Today.Month) + " " + DateTime.Today.Year + " Reports";
            cmbMonthMonthly.SelectedIndex = -1;
            cmbYearMonthly.SelectedIndex = -1;

            getUpdate();
        }

        private void FontSizes()
        {
            cmbMonthMonthly.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategorySingleCat.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbAnnualYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYearSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYearMonthly.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            iconBtnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label5.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label6.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelTitleSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelGraphTitleAnnualReport.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            allCatMonthlyReport.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        string year = DateTime.Today.Year.ToString();
        string month = DateTime.Today.Month.ToString();
        string category = "Programming";

        private string getAllCategoryQuery()
        {
            string allCategoriesQuery = "SELECT CATEGORY.CategoryName, SUM(TASK.timeSpent)/60 \r\n" +
            "FROM TASK \r\n" +
            "JOIN CATEGORY ON TASK.categoryID = CATEGORY.categoryID \r\n" +
            "WHERE TASK.year = @year AND TASK.isDeleted = 0 AND CATEGORY.isDeleted = 0 \r\n" +
            "GROUP BY CATEGORY.categoryName\r\n" +
            "ORDER BY CATEGORY.categoryName ASC";
            return allCategoriesQuery;
        }

        private string getSingleCategoryQuery()
        {
            string singleCategoriesQuery = "SELECT MONTH.monthID, SUM(TASK.timeSpent)/60 \r\n" +
            "FROM TASK \r\n" +
            "JOIN CATEGORY ON TASK.categoryID = CATEGORY.categoryID \r\n" +
            "JOIN MONTH ON TASK.monthID = MONTH.monthID \r\n" +
            "WHERE TASK.year = @year AND TASK.isDeleted = 0 AND CATEGORY.categoryName = @category\r\n" +
            "GROUP BY MONTH.monthID\r\n" +
            "ORDER BY MONTH.monthID ASC";
            return singleCategoriesQuery;
        }

        private string getAllMonthlyCategoryQuery()
        {
            string allCategoriesQuery = "SELECT CATEGORY.CategoryName, SUM(TASK.timeSpent)/60 \r\n" +
            "FROM TASK \r\n" +
            "JOIN CATEGORY ON TASK.categoryID = CATEGORY.categoryID \r\n" +
            "WHERE TASK.year = @year AND TASK.monthID = @month AND TASK.isDeleted = 0 AND CATEGORY.isDeleted = 0 \r\n" +
            "GROUP BY CATEGORY.categoryName\r\n" +
            "ORDER BY CATEGORY.categoryName ASC";
            return allCategoriesQuery;
        }

        private void getUpdate()
        {
            string allCategoryQuery = getAllCategoryQuery();            
            SqlParameter[] allCatparameters = new SqlParameter[]
            {
                new SqlParameter("@year", SqlDbType.VarChar) { Value = year }
            };
            General.CreateChart(chartAnnualReport, allCategoryQuery, allCatparameters, SeriesChartType.Column, "Hours", "");
            labelGraphTitleAnnualReport.Text = DateTime.Today.Year + " Records";

            string singleCategoryQuery = getSingleCategoryQuery();
            SqlParameter[] singleCatparameters = new SqlParameter[]
            {
                new SqlParameter("@year", SqlDbType.VarChar) { Value = year },
                new SqlParameter("@category", SqlDbType.VarChar) { Value = category }
            };
            General.CreateChart(chartSingleCategories, singleCategoryQuery, singleCatparameters, SeriesChartType.Column, "Hours", "");            
            labelTitleSingleCategory.Text = "Programming " + DateTime.Today.Year;

            string AllMonthlyCategoryQuery = getAllMonthlyCategoryQuery();
            SqlParameter[] allMonthlyCatparameters = new SqlParameter[]
            {
                new SqlParameter("@year", SqlDbType.VarChar) { Value = year },
                new SqlParameter("@month", SqlDbType.VarChar) { Value = month },
                new SqlParameter("@category", SqlDbType.VarChar) { Value = category }
            };
            General.CreateChart(chartAllCategoryMonthly, AllMonthlyCategoryQuery, allMonthlyCatparameters, SeriesChartType.Column, "Hours", "");
            allCatMonthlyReport.Text = General.ConventIntToMonth(DateTime.Today.Month) + " " + DateTime.Today.Year + " Reports";
        }

        private void FormGraphs_Load(object sender, EventArgs e)
        {
            dto = bll.Select();
            cmbMonthMonthly.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonthMonthly, "MonthName", "MonthID");
            cmbCategorySingleCat.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategorySingleCat, "CategoryName", "CategoryID");

            cmbAnnualYear.DataSource = dto.Years;
            General.ComboBoxProps(cmbAnnualYear, "Year", "YearID");
            cmbYearSingleCategory.DataSource = dto.Years;
            General.ComboBoxProps(cmbYearSingleCategory, "Year", "YearID");
            cmbYearMonthly.DataSource = dto.Years;
            General.ComboBoxProps(cmbYearMonthly, "Year", "YearID");

            FontSizes();
            getUpdate();            
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            string annualQuery = getAllCategoryQuery();
            string selectedYear = cmbAnnualYear.Text;

            if (cmbAnnualYear.SelectedIndex != -1)
            {
                SqlParameter[] singleCatparameters = new SqlParameter[]
                {
                    new SqlParameter("@year", SqlDbType.VarChar) { Value = selectedYear }
                };
                General.CreateChart(chartAnnualReport, annualQuery, singleCatparameters, SeriesChartType.Column, "Month", "");
                labelGraphTitleAnnualReport.Text = selectedYear + " Report";
            }
            else
            {
                MessageBox.Show("Please select a year from the dropdown.");
            }
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
            string singleCategoryQuery = getSingleCategoryQuery();
            string selectedYear = cmbYearSingleCategory.Text;
            string newCategory;

            if (cmbCategorySingleCat.SelectedIndex != -1 && cmbYearSingleCategory.SelectedIndex != -1)
            {
                newCategory = cmbCategorySingleCat.Text;

                SqlParameter[] singleCatparameters = new SqlParameter[]
                {
                    new SqlParameter("@year", SqlDbType.VarChar) { Value = selectedYear },
                    new SqlParameter("@category", SqlDbType.VarChar) { Value = newCategory }
                };
                General.CreateChart(chartSingleCategories, singleCategoryQuery, singleCatparameters, SeriesChartType.Column, "Month", "");
                labelTitleSingleCategory.Text = newCategory + " " + selectedYear;
            }
            else
            {
                if (cmbCategorySingleCat.SelectedIndex == -1 && cmbYearSingleCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a year and a category from the dropdowns.");
                }
                else if (cmbYearSingleCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a year from the dropdown.");
                }
                else if (cmbCategorySingleCat.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a category from the dropdown.");
                }
                else
                {
                    MessageBox.Show("Unknown error.");
                }
            }
        }

        private void iconBtnSearchMonthly_Click(object sender, EventArgs e)
        {
            string singleMonthQuery = getAllMonthlyCategoryQuery();
            string selectedYear = cmbYearMonthly.Text;
            int selectedMonth = Convert.ToInt32(cmbMonthMonthly.SelectedValue);

            if (cmbYearMonthly.SelectedIndex != -1 && cmbMonthMonthly.SelectedIndex != -1)
            {
                SqlParameter[] singleCatparameters = new SqlParameter[]
                {
                    new SqlParameter("@year", SqlDbType.VarChar) { Value = selectedYear },
                    new SqlParameter("@month", SqlDbType.VarChar) { Value = selectedMonth }
                };
                General.CreateChart(chartAllCategoryMonthly, singleMonthQuery, singleCatparameters, SeriesChartType.Column, "Month", "");
                allCatMonthlyReport.Text = General.ConventIntToMonth(selectedMonth) + " " + selectedYear + " Report";
            }
            else
            {
                if (cmbYearMonthly.SelectedIndex == -1 && cmbMonthMonthly.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a month and a year from the dropdowns.");
                }
                else if(cmbYearMonthly.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a year from the dropdown.");
                }
                else if (cmbMonthMonthly.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a month from the dropdown.");
                }
                else
                {
                    MessageBox.Show("Unknown error.");
                }
            }
        }
    }
}
