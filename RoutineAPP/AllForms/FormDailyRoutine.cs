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
    public partial class FormDailyRoutine : Form
    {
        public FormDailyRoutine()
        {
            InitializeComponent();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public bool isUpdate = false;
        private void FormDailyRoutine_Load(object sender, EventArgs e)
        {
            dateTimePickerRoutine.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            txtSummary.Font = new Font("Segoe UI", 18, FontStyle.Regular);
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            if (isUpdate)
            {
                txtSummary.Text = detail.Summary;
                dateTimePickerRoutine.Value = detail.RoutineDate;
            }
        }
        DailyTaskBLL bll = new DailyTaskBLL();
        public DailyTaskDetailDTO detail = new DailyTaskDetailDTO();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isUpdate)
            {
                DailyTaskDetailDTO dailyTask = new DailyTaskDetailDTO();
                dailyTask.Summary = txtSummary.Text.Trim();
                dailyTask.RoutineDate = dateTimePickerRoutine.Value;
                dailyTask.Day = dateTimePickerRoutine.Value.Day;
                dailyTask.MonthID = dateTimePickerRoutine.Value.Month;
                dailyTask.Year = dateTimePickerRoutine.Value.Year;
                if (bll.Insert(dailyTask))
                {
                    MessageBox.Show("Daily routine was added successfully");
                    txtSummary.Clear();
                    dateTimePickerRoutine.Value = DateTime.Today;
                }
            }
            else if (isUpdate)
            {
                if (detail.RoutineDate == dateTimePickerRoutine.Value && detail.Summary == txtSummary.Text.Trim())
                {
                    MessageBox.Show("There is no change");
                }
                else
                {
                    detail.Summary = txtSummary.Text.Trim();
                    detail.RoutineDate = dateTimePickerRoutine.Value;
                    detail.Day = dateTimePickerRoutine.Value.Day;
                    detail.MonthID = dateTimePickerRoutine.Value.Month;
                    detail.Year = dateTimePickerRoutine.Value.Year;
                    if (bll.Update(detail))
                    {
                        MessageBox.Show("Daily routine was updated successfully");
                        this.Close();
                    }
                }
            }
        }
    }
}
