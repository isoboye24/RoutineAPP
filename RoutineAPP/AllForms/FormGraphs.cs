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
            labelGraphTitleAllCategories.Text = "Programming " + DateTime.Today.Year;
            cmbCategoryAllCat.SelectedIndex = -1;
            cmbMonthAllCategories.SelectedIndex = -1;
            cmbYearAllCategories.SelectedIndex = -1;
        }

        private void FontSizes()
        {
            cmbCategoryAllCat.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategorySingleCat.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonthAllCategories.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYearAllCategories.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbYearSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            btnClearAllCategories.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClearSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnShowAllCategories.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnShowSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label5.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label6.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelTitleSingleCategory.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelGraphTitleAllCategories.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        string year = DateTime.Today.Year.ToString();
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

        private void getUpdate()
        {
            string allCategoryQuery = getAllCategoryQuery();            
            SqlParameter[] allCatparameters = new SqlParameter[]
            {
                new SqlParameter("@year", SqlDbType.VarChar) { Value = year }
            };
            General.CreateChart(chartAllCategories, allCategoryQuery, allCatparameters, SeriesChartType.Column, "Category", "");

            string singleCategoryQuery = getSingleCategoryQuery();
            SqlParameter[] singleCatparameters = new SqlParameter[]
            {
                new SqlParameter("@year", SqlDbType.VarChar) { Value = year },
                new SqlParameter("@category", SqlDbType.VarChar) { Value = category }
            };
            General.CreateChart(chartSingleCategories, singleCategoryQuery, singleCatparameters, SeriesChartType.Column, "Month", "");
            labelTitleSingleCategory.Text = "Programming " + DateTime.Today.Year;
            labelGraphTitleAllCategories.Text = DateTime.Today.Year + " Records";
        }

        private void FormGraphs_Load(object sender, EventArgs e)
        {
            dto = bll.Select();
            cmbCategoryAllCat.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategoryAllCat, "CategoryName", "CategoryID");
            cmbCategorySingleCat.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategorySingleCat, "CategoryName", "CategoryID");
            cmbMonthAllCategories.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonthAllCategories, "MonthName", "MonthID");

            FontSizes();
            getUpdate();            
        }

        private void btnClearSingleCategory_Click(object sender, EventArgs e)
        {
            labelTitleSingleCategory.Text = "Programming " + DateTime.Today.Year;
            cmbCategorySingleCat.SelectedIndex = -1;
            cmbYearSingleCategory.SelectedIndex = -1;
            getUpdate();
        }

        private void btnShowSingleCategory_Click(object sender, EventArgs e)
        {
            string singleCategoryQuery = getSingleCategoryQuery();
            string newCategory;

            if (cmbCategorySingleCat.SelectedIndex != -1)
            {
                int CategoryID = Convert.ToInt32(cmbCategorySingleCat.SelectedValue);
                newCategory = General.ConventIntToCategory(CategoryID);

                SqlParameter[] singleCatparameters = new SqlParameter[]
                {
                    new SqlParameter("@year", SqlDbType.VarChar) { Value = year },
                    new SqlParameter("@category", SqlDbType.VarChar) { Value = newCategory }
                };
                General.CreateChart(chartSingleCategories, singleCategoryQuery, singleCatparameters, SeriesChartType.Column, "Month", "");
                labelTitleSingleCategory.Text = newCategory + " " + DateTime.Today.Year;
            }
            else
            {
                MessageBox.Show("Please select a category from the dropdown.");
            }
        }
    }
}
