using RoutineAPP.Application.Services;
using RoutineAPP.Core.Interfaces;
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
using System.Windows.Forms;
using static RoutineAPP.Helper.ReportHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormMonthlyReports : Form
    {
        IReportService _reportService;
        ICategoryService _categoryService;

        private int month;
        private int year;

        private List<ReportDetailsViewModel> _reportDetailsVM;

        public FormMonthlyReports(IReportService reportService, ICategoryService categoryService)
        {
            InitializeComponent();
            _reportService = reportService;
            _categoryService = categoryService;
        }

        private void iconClose_Click(object sender, EventArgs e)
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

        private void resizeControls()
        {
            GeneralHelper.ApplyRegularFont(12, cmbCategory);
            GeneralHelper.ApplyRegularFont(10, labelTotalCategories);
            GeneralHelper.ApplyBoldFont(12, iconBtnClear, iconBtnSearch, iconBtnClose);
        }

        public void LoadForView(int monthID, int reportYear)
        {
            month = monthID;
            year = reportYear;
        }

        private void loadMonthlyReports()
        {
            var domainList = _reportService.GetReportDetailsByMonth(month, year);

            _reportDetailsVM = domainList.Select(x => new ReportDetailsViewModel
            {
                ReportID = x.ReportID,
                CategoryID = x.CategoryID,
                Category = x.Category,
                TotalTimeUsed = x.TotalTimeUsed,
                PercentageOfUsedTime = x.PercentageOfUsedTime,
                CompletePercentage = x.CompletePercentage
            }).ToList();

            dataGridView1.DataSource = _reportDetailsVM;
            ConfigureReportDetailsGrid(dataGridView1, ReportGridType.ReportDetails);
        }

        private void FormMonthlyReports_Load(object sender, EventArgs e)
        {
            resizeControls();

            loadMonthlyReports();

            fillCombos();
            
            labelTitle.Text = GeneralHelper.ConventIntToMonth(month) + " " + year + " Report";

            RefreshCounts();            
        }

        private void fillCombos()
        {
            var categories = _categoryService.GetAll();
            cmbCategory.DataSource = categories;
            GeneralHelper.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Descending);
        }

        private void RefreshCounts()
        {
            labelTotalCategories.Text = "Categor" + (dataGridView1.Rows.Count > 1 ? "ies : " : "y : ") + dataGridView1.Rows.Count.ToString();

            labelTotalHours.Text = "Total hours : " + _reportService.GetTotalHoursInMonth(month, year);
            labelTotalHoursUsed.Text = "Total hours used : " + _reportService.GetTotalUsedTimeInMonth(month, year);
            labelTotalHoursUnused.Text = "Total hours unused : " + _reportService.GetTotalUnusedTimeInMonth(month, year);
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {            
            if (e.ColumnIndex == 5 && e.Value != null)
            {
                double cellValue;
                if (double.TryParse(e.Value.ToString(), out cellValue))
                {
                    if (cellValue*100 < 5)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Red;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }                    
                    else if (cellValue * 100 >= 5 && cellValue * 100 <= 10)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.Yellow;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else if (cellValue * 100 > 10 && cellValue * 100 <= 25)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGreen;
                            cell.Style.ForeColor = Color.White;
                        }
                    }
                    else if (cellValue * 100 > 25)
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = Color.DarkGoldenrod;
                            cell.Style.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                            cell.Style.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                        }
                    }
                }
            }
        }

       
        private void ClearFilters()
        {
            cmbCategory.SelectedIndex = -1;
            loadMonthlyReports();
            RefreshCounts();
        }
        
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Draw row numbers on the row header
            using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
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

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            int searchedCategory;
            List<ReportDetailsViewModel> filtered = new List<ReportDetailsViewModel>();

            if (cmbCategory.SelectedIndex != -1)
            {
                searchedCategory = Convert.ToInt32(cmbCategory.SelectedValue);
                filtered = _reportDetailsVM.Where(x => x.CategoryID == searchedCategory).ToList();
            }           
            else
            {
                MessageBox.Show("Please select category");
            }
            dataGridView1.DataSource = filtered;
            RefreshCounts();
        }
    }
}
