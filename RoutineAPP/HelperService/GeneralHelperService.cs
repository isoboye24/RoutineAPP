using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoutineAPP.HelperService
{
    public class GeneralHelperService
    {
        public static void ApplyBoldFont11(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold);
            }
        }
        public static void ApplyRegularFont11(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Regular);
            }
        }
        public static void ApplyBoldFont12(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold);
            }
        }
        public static void ApplyRegularFont12(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular);
            }
        }

        public static void ApplyBoldFont14(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);
            }
        }
        public static void ApplyRegularFont14(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Regular);
            }
        }
        public static void ApplyBoldFont16(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 16, FontStyle.Bold);
            }
        }
        public static void ApplyRegularFont16(params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Font = new System.Drawing.Font("Segoe UI", 16, FontStyle.Regular);
            }
        }

        public static void SetVisibleColumns(DataGridView grid, params string[] visibleColumns)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Visible = visibleColumns.Contains(column.Name);
                column.HeaderCell.Style.Font = new System.Drawing.Font("Segoe UI", 16, FontStyle.Bold);
            }
        }

        public static void RenameColumns(DataGridView grid, Dictionary<string, string> mappings)
        {
            foreach (var map in mappings)
            {
                if (grid.Columns.Contains(map.Key))
                    grid.Columns[map.Key].HeaderText = map.Value;
            }
        }

        // Generic reflection mapping pattern
        public static T MapFromGrid<T>(DataGridView grid, int rowIndex) where T : new()
        {
            var row = grid.Rows[rowIndex];
            var obj = new T();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!grid.Columns.Contains(prop.Name))
                    continue;

                var value = row.Cells[prop.Name].Value;

                if (value == null || value == DBNull.Value)
                    continue;

                var convertedValue = Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                prop.SetValue(obj, convertedValue);
            }

            return obj;
        }

    }
}
