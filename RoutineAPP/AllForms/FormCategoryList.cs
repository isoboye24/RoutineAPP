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
    public partial class FormCategoryList : Form
    {
        public FormCategoryList()
        {
            InitializeComponent();
        }
        CategoryDTO dto = new CategoryDTO();
        CategoryBLL bll = new CategoryBLL();
        private void FormCategoryList_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            labelTotalCategory.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            iconBtnAdd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnDelete.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            iconBtnEdit.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            dto = bll.Select();
            dataGridView1.DataSource = dto.Categories;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Categories";
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            RefreshDataCounts();
        }
        private void ClearFilters()
        {
            txtCategory.Clear();
            bll = new CategoryBLL();
            dto = bll.Select();
            dataGridView1.DataSource = dto.Categories;
            RefreshDataCounts();
        }
        private void RefreshDataCounts()
        {
            labelTotalCategory.Text = dataGridView1.RowCount.ToString();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormCategory open = new FormCategory();
            this.Hide();
            open.ShowDialog();
            this.Visible = true;
            ClearFilters();            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.CategoryID == 0)
            {
                MessageBox.Show("Please choose a task from the table");
            }
            else
            {
                FormCategory open = new FormCategory();
                open.isUpdate = true;
                open.detail = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }
            
        }
        CategoryDetailDTO detail = new CategoryDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail = new CategoryDetailDTO();
            detail.CategoryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.CategoryName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (detail.CategoryID == 0)
            {
                MessageBox.Show("Please select a category");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure?","Warning!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (bll.Delete(detail))
                    {
                        MessageBox.Show("Category was deleted successfully");
                        ClearFilters();
                    }
                }
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {
            List<CategoryDetailDTO> list = dto.Categories;
            list = list.Where(x => x.CategoryName.Contains(txtCategory.Text.Trim())).ToList();
            dataGridView1.DataSource = list;

        }

        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            FormCategory open = new FormCategory();
            this.Hide();
            open.ShowDialog();
            this.Visible = true;
            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            if (detail.CategoryID == 0)
            {
                MessageBox.Show("Please choose a task from the table");
            }
            else
            {
                FormCategory open = new FormCategory();
                open.isUpdate = true;
                open.detail = detail;
                this.Hide();
                open.ShowDialog();
                this.Visible = true;
                ClearFilters();
            }

        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            if (detail.CategoryID == 0)
            {
                MessageBox.Show("Please select a category");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Warning!", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (bll.Delete(detail))
                    {
                        MessageBox.Show("Category was deleted successfully");
                        ClearFilters();
                    }
                }
            }
        }
    }
}
