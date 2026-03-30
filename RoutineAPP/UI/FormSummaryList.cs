using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static RoutineAPP.Helper.ReportHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormCommentList : Form
    {
        private readonly IDailyRoutineService _dailyRoutineService;
        private readonly IMonthService _monthlyService;
        private bool _isView = false;

        List<DailyRoutineDTO> _dailyRoutineVM;
        public FormCommentList(IDailyRoutineService dailyRoutineService, IMonthService monthService)
        {
            InitializeComponent();
            _dailyRoutineService = dailyRoutineService;
            _monthlyService = monthService;
        }
        
        private int year => DateTime.Now.Year;

        private void resizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, label4);
            GeneralHelper.ApplyRegularFont(12, txtDay, cmbYear, cmbMonth);
            GeneralHelper.ApplyRegularFont(10, labelTotalComments);
        }

        private void fillCombos()
        {
            var monthList = _monthlyService.GetAll();
            cmbMonth.DataSource = monthList;
            GeneralHelper.ComboBoxProps(cmbMonth, "MonthName", "MonthID");

            var yearList = _dailyRoutineService.GetOnlyYears();
            cmbYear.DataSource = yearList;
            GeneralHelper.ComboBoxProps(cmbYear, "Year", "YearID");
        }

        private void loadComments(int year)
        {
            dataGridView1.DataSource = _dailyRoutineService.GetComments(year);
            CommentHelper.ConfigureCommentGrid(dataGridView1, CommentHelper.CommentGridType.Basic);
        }

        private void FormCommentList_Load(object sender, EventArgs e)
        {
            resizeControls();

            fillCombos();

            loadComments(year);

            RefreshDataCounts();
        }

        private void RefreshDataCounts()
        {
            labelTotalComments.Text = dataGridView1.RowCount.ToString();
        }

        private void ClearFilters()
        {
            txtComment.Clear();
            txtDay.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;

            loadComments(year);

            RefreshDataCounts();
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {
            string search = txtComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                dataGridView1.DataSource = _dailyRoutineVM;
            }
            else
            {
                var filtered = _dailyRoutineVM
                    .Where(x => !string.IsNullOrEmpty(x.Summary) &&
                                x.Summary.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dataGridView1.DataSource = filtered;
            }

            RefreshDataCounts();
        }

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            int searchDate = Convert.ToInt32(txtDay.Text.Trim());
            var filtered = _dailyRoutineVM.Where(x => x.Day == searchDate).ToList();
            dataGridView1.DataSource = filtered;
            RefreshDataCounts();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<DailyRoutineDTO>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormDailyRoutine(_dailyRoutineService);
            form.LoadForCommentView(true, selected);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            int searchedMonth;
            int searchedYear;
            List<DailyRoutineDTO> filtered = new List<DailyRoutineDTO>();

            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                filtered = _dailyRoutineVM.Where(x => x.MonthID == searchedMonth).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                loadComments(searchedYear);
                filtered = _dailyRoutineVM.Where(x => x.Year == searchedYear).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                loadComments(searchedYear);
                filtered = _dailyRoutineVM.Where(x => x.Year == searchedYear && x.MonthID == searchedMonth).ToList();
            }
            else
            {
                MessageBox.Show("Please at least a month or year");
            }
            dataGridView1.DataSource = filtered;
            RefreshDataCounts();
        }

        private void txtDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }
    }
}
