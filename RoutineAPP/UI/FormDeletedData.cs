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
    public partial class FormDeletedData : Form
    {
        public FormDeletedData()
        {
            InitializeComponent();
        }

        private void FormDeletedData_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);            
            cmbDeletedData.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            iconBtnRetrieve.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
        }

    }
}
