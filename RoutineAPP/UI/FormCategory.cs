using RoutineAPP.Application.Interfaces;
using RoutineAPP.Core.Entities;
using RoutineAPP.Helper;
using System;
using System.Windows.Forms;

namespace RoutineAPP.AllForms
{
    public partial class FormCategory : Form
    {
        private readonly ICategoryService _categoryService;
        private int _categoryId = 0;
        public FormCategory(ICategoryService categoryService)
        {
            InitializeComponent();
            _categoryService = categoryService;
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, labelTitle, iconBtnClose, iconBtnSave);
        }

        public void LoadForEdit(int id, string name)
        {
            _categoryId = id;
            txtCategory.Text = name;
        }

        private void FormCategory_Load(object sender, EventArgs e)
        {
            ResizeControls();            
        }               

        private void iconBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var name = txtCategory.Text.Trim();

                if (_categoryId == 0)
                {
                    var category = new Category(name);
                    _categoryService.Create(category);
                    MessageBox.Show("Category created successfully!");
                }
                else
                {
                    var category = new Category(name);
                    category.SetId(_categoryId);
                    _categoryService.Update(category);
                    MessageBox.Show("Category updated successfully!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void iconBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
