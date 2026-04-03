using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Core.Entities;
using RoutineAPP.Helper;
using System;
using System.Windows.Forms;

namespace RoutineAPP.AllForms
{
    public partial class FormDailyRoutine : Form
    {
        private readonly IDailyRoutineService _dailyService;
        private DailyRoutineDTO _dailyRoutineDTO;
        private DailyRoutineDTO _dailyRoutineViewDTO;

        private int _routineId = 0;
        private bool _isView = false;
        private DateTime _date;
        private string _summary;
        private bool _isUpdate = false;

        public FormDailyRoutine(IDailyRoutineService dailyService)
        {
            InitializeComponent();
            _dailyService = dailyService;
        }

        private void ResizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, iconBtnClose, iconBtnSave);
            GeneralHelper.ApplyBoldFont(14, labelTitle, dateTimePickerRoutine);
            GeneralHelper.ApplyRegularFont(18, txtSummary);
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

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadForEdit(DailyRoutineDTO dailyRoutineDTO)
        {
            _dailyRoutineDTO = dailyRoutineDTO;
            _isUpdate = true;
        }

        public void LoadForCommentView(bool isView, DailyRoutineDTO dailyRoutineViewDTO)
        {
            _isView = isView;
            _dailyRoutineViewDTO = dailyRoutineViewDTO;
        }


        public bool isSummaryList = false;
        private void FormDailyRoutine_Load(object sender, EventArgs e)
        {
            if (_isView)
            {
                dateTimePickerRoutine.Hide();
                label3.Hide();
                txtSummary.ReadOnly = true;
                txtSummary.Text = _dailyRoutineViewDTO.Summary;
                labelTitle.Text = "Routine on " + _dailyRoutineViewDTO.Day + "." + _dailyRoutineViewDTO.MonthName + "." + _dailyRoutineViewDTO.Year;
            }
            else if (_isUpdate)
            {
                dateTimePickerRoutine.Value = _dailyRoutineDTO.RoutineDate;
                txtSummary.Text = _dailyRoutineDTO.Summary;
                labelTitle.Text = "Edit Routine on " + _dailyRoutineDTO.Day + "." + _dailyRoutineDTO.MonthName + "." + _dailyRoutineDTO.Year;
            }

            ResizeControls();            
        }

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string summary = txtSummary.Text.Trim();
                DateTime date = dateTimePickerRoutine.Value;

                if (!_isUpdate)
                {
                    var routine = new DailyRoutine(date, summary);
                    _dailyService.Create(routine);
                    MessageBox.Show("Daily routine created successfully!");
                }
                else
                {
                    var routine = DailyRoutine.Rehydrate(_dailyRoutineDTO.Id, date, summary);
                    routine.SetId(_routineId);
                    _dailyService.Update(routine);
                    MessageBox.Show("Daily routine updated successfully!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
