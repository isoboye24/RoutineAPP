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
    public partial class FormCategoryList : Form
    {
        public FormCategoryList()
        {
            InitializeComponent();
        }

        private void FormCategoryList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelMaleChildren.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            txtCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnAdd.Font = new Font("Segoe UI", 12, FontStyle.Bold);            
            btnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);            
            btnUpdate.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnView.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
        }
    }
}
