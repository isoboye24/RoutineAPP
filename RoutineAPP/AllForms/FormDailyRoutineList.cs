﻿using RoutineAPP.BLL;
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
    public partial class FormDailyRoutineList : Form
    {
        public FormDailyRoutineList()
        {
            InitializeComponent();
        }

        private void RefreshDataCounts()
        {
            labelTotalRoutine.Text = dataGridView1.RowCount.ToString();
        }

        DailyTaskBLL bll = new DailyTaskBLL();
        DailyTaskDTO dto = new DailyTaskDTO();
        private void FormDailyRoutineList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtDay.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            cmbYear.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            iconBtnAdd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnEdit.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");
            cmbYear.DataSource = dto.Years;
            General.ComboBoxProps(cmbYear, "Year", "YearID");

            dataGridView1.DataSource = dto.DailyRoutines;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Day";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].HeaderText = "Month";
            dataGridView1.Columns[6].HeaderText = "Year";
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            RefreshDataCounts();
        }
        DailyTaskDetailDTO detail = new DailyTaskDetailDTO();
        

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            List<DailyTaskDetailDTO> list = dto.DailyRoutines;
            list = list.Where(x => x.Day.ToString().Contains(txtDay.Text.Trim())).ToList();
            dataGridView1.DataSource = list;
            RefreshDataCounts();
        }

        private void ClearFilters()
        {
            txtDay.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            bll = new DailyTaskBLL();
            dto = bll.Select();
            dataGridView1.DataSource = dto.DailyRoutines;
            RefreshDataCounts();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new DailyTaskDetailDTO();
            detail.DailyTaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.RoutineDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.Summary = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Day = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.MonthName = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            detail.Year = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            txtSummary.Text = detail.Summary;
        }

        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            FormDailyRoutine open = new FormDailyRoutine();
            this.Hide();
            open.ShowDialog();
            this.Visible = true;
            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            if (detail.DailyTaskID == 0)
            {
                MessageBox.Show("Please choose a routine from the table");
            }
            else
            {
                FormDailyRoutine open = new FormDailyRoutine();
                open.isUpdate = true;
                open.detail = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void iconBtnView_Click(object sender, EventArgs e)
        {
            if (detail.DailyTaskID == 0)
            {
                MessageBox.Show("Please choose a routine from the table");
            }
            else
            {
                FormTaskList open = new FormTaskList();
                open.detailDailyRoutine = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            if (detail.DailyTaskID == 0)
            {
                MessageBox.Show("Please choose a routine from the table");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Warning!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (bll.Delete(detail))
                    {
                        MessageBox.Show("Daily Routine was deleted successfully");
                        ClearFilters();
                    }
                }
            }
        }

        private void iconBtnSearch_Click(object sender, EventArgs e)
        {
            List<DailyTaskDetailDTO> list = dto.DailyRoutines;
            if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex == -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            }
            else if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
            {
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue) && x.Year == Convert.ToInt32(cmbYear.Text)).ToList();
            }
            else if (cmbMonth.SelectedIndex == -1 && cmbYear.SelectedIndex != -1)
            {
                list = list.Where(x => x.Year == Convert.ToInt32(cmbYear.Text)).ToList();
            }
            dataGridView1.DataSource = list;
            RefreshDataCounts();
        }

        private void iconBtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }
    }
}
