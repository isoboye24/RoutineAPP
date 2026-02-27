using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RoutineAPP.AllForms
{
    public partial class FormSummaryList : Form
    {
        private readonly ICommentService _commentService;
        private readonly IDailyRoutineService _dailyRoutineService;
        private readonly IMonthService _monthlyService;
        private bool _isView = false;

        List<DailyRoutineViewModel> _dailyRoutineVM;
        public FormSummaryList(ICommentService commentService, IDailyRoutineService dailyRoutineService, IMonthService monthService)
        {
            InitializeComponent();
            _commentService = commentService;
            _dailyRoutineService = dailyRoutineService;
            _monthlyService = monthService;
        }
        

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

        private void loadSummaries()
        {
            var summaries = _commentService.GetComments();

            _dailyRoutineVM = summaries.Select(x => new DailyRoutineViewModel
            {
                Id = x.Id,
                RoutineDate = x.RoutineDate,
                Summary = x.Summary,
                Day = x.Day,
                MonthID = x.MonthID,
                MonthName = x.MonthName,
                Year = x.Year
            }).ToList();
        }

        private void FormCommentList_Load(object sender, EventArgs e)
        {
            resizeControls();

            fillCombos();

            loadSummaries();

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
            
            loadSummaries();

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
            var selected = GeneralHelper.GetSelected<DailyRoutineViewModel>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormDailyRoutine(_dailyRoutineService);
            form.LoadForCommentView(_isView = true, selected.RoutineDate, selected.Summary);
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
            List<DailyRoutineViewModel> filtered = new List<DailyRoutineViewModel>();

            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                filtered = _dailyRoutineVM.Where(x => x.MonthID == searchedMonth).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
                filtered = _dailyRoutineVM.Where(x => x.Year == searchedYear).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                searchedMonth = Convert.ToInt32(cmbMonth.SelectedValue);
                searchedYear = Convert.ToInt32(cmbYear.SelectedValue);
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
