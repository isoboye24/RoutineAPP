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
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            labelTotalCategories.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.SelectMonthlyReports(routineDetail.MonthID, routineDetail.Year);
            dataGridView1.DataSource = dto.MonthlyReports;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Category";
            dataGridView1.Columns[3].HeaderText = "Total Time Used";
            dataGridView1.Columns[4].HeaderText = "% in 2 dp";
            dataGridView1.Columns[5].HeaderText = "Complete %";
            dataGridView1.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            labelTotalHours.Text = "Total hours in " + General.ConventIntToMonth(routineDetail.MonthID) + " : "+ bll.SelectTotalHoursInMonth(routineDetail.MonthID, routineDetail.Year).ToString();
            labelTotalHoursUsed.Text = "Total hours used : " + bll.SelectTotalHoursUsedInMonth(routineDetail.MonthID, routineDetail.Year);
            labelTotalHoursUnused.Text = "Total hours unused : " + bll.SelectTotalHoursUnusedInMonth(routineDetail.MonthID, routineDetail.Year);
            labelTitle.Text = routineDetail.MonthName + " " + routineDetail.Year + " Report";
            cmbCategory.DataSource = dto.Categories;
            General.ComboBoxProps(cmbCategory, "CategoryName", "CategoryID");

            RefreshCounts();
        }

        private void RefreshCounts()
        {
            labelTotalCategories.Text = "Categor" + (dataGridView1.Rows.Count > 1? "ies : " : "y : ")  + dataGridView1.Rows.Count.ToString();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Descending);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<ReportsDetailDTO> list = dto.MonthlyReports;
            if (cmbCategory.SelectedIndex != -1)
            {
                list = list.Where(x => x.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue)).ToList();
            }
            dataGridView1.DataSource = list;
            RefreshCounts();
        }
        ReportsDetailDTO detail = new ReportsDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new ReportsDetailDTO();
            detail.ReportID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.CategoryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.Category = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.TotalTimeUsed = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.PercentageOfUsedTime = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            detail.TotalTimeForFormatting = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
        }
        private void ClearFilters()
        {
            cmbCategory.SelectedIndex = -1;
            bll = new ReportsBLL();
            dto = bll.SelectMonthlyReports(routineDetail.MonthID, routineDetail.Year);
            dataGridView1.DataSource = dto.MonthlyReports;
            RefreshCounts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
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
    }
}
