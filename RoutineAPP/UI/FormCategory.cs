using RoutineAPP.BLL;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.DAL.DTO;
using RoutineAPP.HelperService;
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
        private readonly ICategoryService _service;
        private int _categoryId = 0;
        public FormCategory(ICategoryService service)
        {
            InitializeComponent();
            _service = service;
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResizeControls()
        {
            GeneralHelperService.ApplyBoldFont(12, label1, labelTitle, iconBtnClose, iconBtnSave);
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
                if (_categoryId == 0)
                    _service.Create(txtCategory.Text);
                else
                    _service.Update(_categoryId, txtCategory.Text);

                MessageBox.Show("Operation successful");
                this.Close();
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
