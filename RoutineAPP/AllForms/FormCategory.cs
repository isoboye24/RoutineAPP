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
    public partial class FormCategory : Form
    {
        public FormCategory()
        {
            InitializeComponent();
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCategory_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            txtCategory.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            if (isUpdate)
            {
                txtCategory.Text = detail.CategoryName;
            }
        }
        CategoryBLL bll = new CategoryBLL();
        public bool isUpdate = false;
        public CategoryDetailDTO detail = new CategoryDetailDTO();
        private void btnSave_Click(object sender, EventArgs e)
        {
            int checkCategory = bll.CheckCategory(txtCategory.Text.Trim());
            if (txtCategory.Text.Trim()=="")
            {
                MessageBox.Show("Category is empty");
            }            
            else
            {
                if (checkCategory > 0)
                {
                    MessageBox.Show("This category already exists");
                }
                else if(!isUpdate)
                {
                    CategoryDetailDTO category = new CategoryDetailDTO();
                    category.CategoryName = txtCategory.Text.Trim();
                    if (bll.Insert(category))
                    {
                        MessageBox.Show("Category was added successfully");
                        txtCategory.Clear();
                    }
                }
                else if (isUpdate)
                {
                    if (detail.CategoryName == txtCategory.Text.Trim())
                    {
                        MessageBox.Show("There is no change");
                    }
                    else
                    {
                        detail.CategoryName = txtCategory.Text.Trim();
                        if (bll.Update(detail))
                        {
                            MessageBox.Show("Category was updated successfully");
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
