using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private List<DailyRoutineDTO> _dailyRoutineDTODefault;
        private List<DailyRoutineDTO> _dailyRoutineDTOAllLists;

        private int year = DateTime.Today.Year;
        private string allYearRange = ("2024 - " + DateTime.Today.Year).ToString();

        public FormDailyRoutineList(IMonthService monthService, IDailyRoutineService dailyService, ITaskService taskService, ICategoryService categoryService, IReportService reportService)
        {
            InitializeComponent();
            _monthService = monthService;
            _dailyService = dailyService;
            _taskService = taskService;
            _categoryService = categoryService;
            _reportService = reportService;
        }

        private void resizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, label4, iconBtnAdd, iconBtnClear, iconBtnDelete, iconBtnSearch, iconBtnEdit);
            GeneralHelper.ApplyRegularFont(14, txtDay, cmbYear, cmbMonth);
        }

        private void fillCombos()
        {
            cmbMonth.DataSource = _monthService.GetAll();
            GeneralHelper.ComboBoxProps(cmbMonth, "MonthName", "MonthID");

            cmbYear.DataSource = _dailyService.GetOnlyYears();
            GeneralHelper.ComboBoxProps(cmbYear, "Year", "YearID");
        }

        private void FormDailyRoutineList_Load(object sender, EventArgs e)
        {
            resizeControls();

            fillCombos();

            loadDailyRoutine(year);
            refreshDataCounts(year.ToString());
        }

        private void refreshDataCounts(string year)
        {
            labelTotalRoutine.Text = dataGridView1.RowCount + " Day" + (dataGridView1.RowCount > 1 ? "s" : "") + " in " + year.ToString();
        }

        private void loadDailyRoutine(int year)
        {
            _dailyRoutineDTOAllLists = _dailyService.GetAll();
            _dailyRoutineDTODefault = _dailyService.GetAllByYear(year);
            dataGridView1.DataSource = _dailyRoutineDTODefault;
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
            if (txtDay.Text.Trim() != "")
            {
                int search = Convert.ToInt32(txtDay.Text.Trim());
                var filtered = _dailyRoutineDTOAllLists.Where(x => x.Day == search).ToList();
                dataGridView1.DataSource = filtered;

                refreshDataCounts("2024 - " + year);
            }            
        }

        private void ClearFilters()
        {
            txtDay.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            loadDailyRoutine(year);

            refreshDataCounts(year.ToString());
        }


        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            var form = new FormDailyRoutine(_dailyService);
            form.ShowDialog();

            ClearFilters();
        }

        private DailyRoutineDTO GetSelected()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            return dataGridView1.CurrentRow.DataBoundItem as DailyRoutineDTO;
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormDailyRoutine(_dailyService);
            form.LoadForEdit(selected);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
            var selected = GetSelected();

            if (selected == null)
            {
                MessageBox.Show("Please choose a routine from the table.");
                return;
            }

            var form = new FormTaskList(_taskService, _categoryService, _reportService);
            form.LoadForView(selected);
            form.ShowDialog();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelected();
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
            if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please select at least a month or year");
                return;
            }
            else
            {
                var query = _dailyRoutineDTOAllLists.AsEnumerable();
                int selectedMonth = cmbMonth.SelectedIndex != -1 ? Convert.ToInt32(cmbMonth.SelectedValue) : -1;
                int selectedYear = cmbYear.SelectedIndex != -1 ? Convert.ToInt32(cmbYear.SelectedValue) : -1;
                bool hasYear = cmbYear.SelectedIndex != -1;

                if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
                {
                    query = query.Where(x => x.Year == selectedYear && x.MonthID == selectedMonth);
                }
                else if (cmbYear.SelectedIndex != -1)
                {
                    query = query.Where(x => x.Year == selectedYear);
                }
                else
                {
                    query = query.Where(x => x.MonthID == selectedMonth);
                }

                var filtered = query.ToList();

                dataGridView1.DataSource = filtered;

                refreshDataCounts(hasYear ? selectedYear.ToString() : allYearRange);
            }
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
