using RoutineAPP.BLL;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.DAL.DTO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RoutineAPP.AllForms
{
    public partial class FormDailyRoutine : Form
    {
        private readonly IDailyRoutineService _dailyService;

        private int _routineId = 0;
        private bool _isView = false;
        private DateTime _date;
        private string _summary;

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

        public void LoadForEdit(int id, DateTime date, string summary)
        {
            _routineId = id;
            txtSummary.Text = summary;
            dateTimePickerRoutine.Value = date;
        }

        public void LoadForCommentView(bool isView, DateTime date, string summary)
        {
            _isView = isView;
            _date = date;
            _summary = summary;
        }


        public bool isSummaryList = false;
        private void FormDailyRoutine_Load(object sender, EventArgs e)
        {
            if (_isView)
            {
                dateTimePickerRoutine.Hide();
                label3.Hide();
                txtSummary.ReadOnly = true;
                txtSummary.Text = _summary;
                labelTitle.Text = "Routine on " + _date.Day + "." + _date.Month + "." + _date.Year;
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
                if (_routineId == 0)
                    _dailyService.Create(dateTimePickerRoutine.Value, txtSummary.Text.Trim());
                else
                    _dailyService.Update(_routineId, dateTimePickerRoutine.Value, txtSummary.Text.Trim());

                MessageBox.Show("Operation successful");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
