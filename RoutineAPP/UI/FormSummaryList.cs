using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Forms;
using static RoutineAPP.Helper.ReportHelper;

namespace RoutineAPP.AllForms
{
    public partial class FormCommentList : Form
    {
        private readonly IDailyRoutineService _dailyRoutineService;
        private readonly IMonthService _monthlyService;

        List<DailyRoutineDTO> _dailyRoutineDTODefault;
        List<DailyRoutineDTO> _dailyRoutineDTOAllLists;
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
            _dailyRoutineDTODefault = _dailyRoutineService.GetComments(year);
            _dailyRoutineDTOAllLists = _dailyRoutineService.GetAllComments();
            dataGridView1.DataSource = _dailyRoutineDTODefault;
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
                dataGridView1.DataSource = _dailyRoutineDTODefault;
            }
            else
            {
                var filtered = _dailyRoutineDTOAllLists
                    .Where(x => !string.IsNullOrEmpty(x.Summary) &&
                                x.Summary.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dataGridView1.DataSource = filtered;
            }

            RefreshDataCounts();
        }

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            if (txtDay.Text.Trim() != "")
            {
                int searchDate = Convert.ToInt32(txtDay.Text.Trim());
                var filtered = _dailyRoutineDTOAllLists.Where(x => x.Day == searchDate).ToList();
                dataGridView1.DataSource = filtered;
                RefreshDataCounts();
            }            
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
                RefreshDataCounts();
            }            
        }

        private void txtDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = GeneralHelper.isNumber(e);
        }
    }
}
