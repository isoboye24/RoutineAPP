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
    public partial class FormTaskList : Form
    {
        public FormTaskList()
        {
            InitializeComponent();
        }
        private void ClearFilters()
        {

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormTask open = new FormTask();
            this.Hide();
            open.ShowDialog();
            this.Visible = true;
            ClearFilters();
        }
        TaskBLL bll = new TaskBLL();
        TaskDTO dto = new TaskDTO();
        private void FormTaskList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label3.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtDay.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            txtYear.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbMonth.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            cmbCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnAdd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSearch.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnUpdate.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnView.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            cmbMonth.DataSource = dto.Months;
            General.ComboBoxProps(cmbMonth, "MonthName", "MonthID");
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
        }
    }
}
