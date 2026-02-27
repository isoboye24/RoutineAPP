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
using System.Windows.Forms;
using static RoutineAPP.Helper.RoutineHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormDailyRoutineList : Form
    {
        private readonly IMonthService _monthService;
        private readonly IDailyRoutineService _dailyService;
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IReportService _reportService;

        private List<DailyRoutineViewModel> _dailyRoutineVM;

        public FormDailyRoutineList(IMonthService monthService, IDailyRoutineService dailyService, ITaskService taskService, ICategoryService categoryService)
        {
            InitializeComponent();
            _monthService = monthService;
            _dailyService = dailyService;
            _taskService = taskService;
            _categoryService = categoryService;
        }

        private void resizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, label4, iconBtnAdd, iconBtnClear, iconBtnDelete, iconBtnSearch, iconBtnEdit);
            GeneralHelper.ApplyRegularFont(14, txtDay, cmbYear, cmbMonth);
        }

        private void fillCombos()
        {
            var months = _monthService.GetAll();
            cmbMonth.DataSource = months;
            GeneralHelper.ComboBoxProps(cmbMonth, "Name", "Id");

            cmbYear.DataSource = _dailyService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYear, "Year", "YearID");
        }

        private void FormDailyRoutineList_Load(object sender, EventArgs e)
        {
            resizeControls();

            fillCombos();

            loadDailyRoutine();
            refreshDataCounts();
        }

        private void refreshDataCounts()
        {
            labelTotalRoutine.Text = dataGridView1.RowCount + " Day" + (dataGridView1.RowCount > 1 ? "s" : "");
        }

        private void loadDailyRoutine()
        {
            var domainList = _dailyService.GetAll();

            _dailyRoutineVM = domainList
                .Select(x => new DailyRoutineViewModel
                {
                    Id = x.Id,
                    RoutineDate = x.Date,
                    Summary = x.Summary,
                    Day = x.Date.Day,
                    MonthID = x.Date.Month,
                    MonthName = GeneralHelper.ConventIntToMonth(x.Date.Month),
                    Year = x.Date.Year

                })
                .ToList();

            dataGridView1.DataSource = _dailyRoutineVM;
            ConfigureDailyRoutineGrid(dataGridView1, RoutineGridType.Basic);
        }


        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }

        private void txtDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            int search = Convert.ToInt32(txtDay.Text.Trim());
            var filtered = _dailyRoutineVM.Where(x => x.Day == search).ToList();
            dataGridView1.DataSource = filtered;
        }

        private void ClearFilters()
        {
            txtDay.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            loadDailyRoutine();

            refreshDataCounts();
        }


        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            var form = new FormDailyRoutine(_dailyService);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<DailyRoutineViewModel>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormDailyRoutine(_dailyService);
            form.LoadForEdit(selected.Id, selected.RoutineDate, selected.Summary);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<DailyRoutineViewModel>(dataGridView1);

            if (selected == null)
            {
                MessageBox.Show("Please choose a routine from the table.");
                return;
            }

            var form = new FormTaskList(_taskService, _categoryService, _reportService);
            form.LoadForView(selected.Id, selected.RoutineDate);
            form.ShowDialog();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GeneralHelper.GetSelected<DailyRoutineViewModel>(dataGridView1);
            if (selected == null)
            {
                MessageBox.Show("Please select a daily routine.");
                return;
            }

            var result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _dailyService.Delete(selected.Id);
                ClearFilters();
            }
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

            refreshDataCounts();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
