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

namespace RoutineAPP.AllForms
{
    public partial class FormCategoryList : Form
    {
        private readonly ICategoryService _service;
        private List<CategoryViewModel> _categories;
        public FormCategoryList(ICategoryService service)
        {
            InitializeComponent();
            _service = service;
        }

        private void resizeControls()
        {
            GeneralHelper.ApplyBoldFont(12, label1, txtCategory, iconBtnAdd, iconBtnDelete, iconBtnEdit);
            GeneralHelper.ApplyRegularFont(11, label2, labelTotalCategory);
        }
        private void FormCategoryList_Load(object sender, EventArgs e)
        {
            resizeControls();

            LoadCategories();

            RefreshDataCounts();
        }

        private void LoadCategories()
        {
            var domainList = _service.GetAll();

            _categories = domainList
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Category = x.Name
                })
                .ToList();

            dataGridView1.DataSource = _categories;

            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Category"].HeaderText = "Category";
        }


        private void ClearFilters()
        {
            txtCategory.Clear();
            LoadCategories();
            RefreshDataCounts();
        }
        private void RefreshDataCounts()
        {
            labelTotalCategory.Text = dataGridView1.RowCount.ToString();
        }

        private CategoryViewModel GetSelected()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            return dataGridView1.CurrentRow.DataBoundItem as CategoryViewModel;
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {
            string search = txtCategory.Text.Trim().ToLower();
            var filtered = _categories.Where(x => x.Category.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            dataGridView1.DataSource = filtered;
        }

        private void iconBtnAdd_Click(object sender, EventArgs e)
        {
            var form = new FormCategory(_service);
            form.ShowDialog();
            ClearFilters();
        }

        private void iconBtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var form = new FormCategory(_service);
            form.LoadForEdit(selected.Id, selected.Category);
            form.ShowDialog();

            ClearFilters();
        }

        private void iconBtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            var result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _service.Delete(selected.Id);
                ClearFilters();
            }
        }
    }
}
