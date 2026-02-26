using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace RoutineAPP
{
    public class General
    {
        public static bool isNumber(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void ComboBoxProps(ComboBox cmb, string name, string ID)
        {
            cmb.DisplayMember = name;
            cmb.ValueMember = ID;
            cmb.SelectedIndex = -1;
        }
        public static string ConventIntToMonth(int month)
        {
            if (month == 1)
            {
                return "January";
            }
            else if (month == 2)
            {
                return "February";
            }
            else if (month == 3)
            {
                return "March";
            }
            else if (month == 4)
            {
                return "April";
            }
            else if (month == 5)
            {
                return "May";
            }
            else if (month == 6)
            {
                return "June";
            }
            else if (month == 7)
            {
                return "July";
            }
            else if (month == 8)
            {
                return "August";
            }
            else if (month == 9)
            {
                return "September";
            }
            else if (month == 10)
            {
                return "October";
            }
            else if (month == 11)
            {
                return "November";
            }
            else if (month == 12)
            {
                return "December";
            }
            else
            {
                return "Unknown month";
            }
        }

        public static void StyleDataGridView(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = Color.Brown;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Brown;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.EnableHeadersVisualStyles = false;
        }

        public static T GetSelected<T>(DataGridView grid) where T : class
        {
            if (grid.CurrentRow == null)
                return null;

            return grid.CurrentRow.DataBoundItem as T;
        }
    }
}
