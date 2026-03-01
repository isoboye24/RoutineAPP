using RoutineAPP.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.Helper
{
    public class CategoryHelper
    {
        public enum CategoryGridType
        {
            Basic
        }

        public static void ConfigureCategoryGrid(DataGridView grid, CategoryGridType type)
        {
            switch (type)
            {
                case CategoryGridType.Basic:
                    GeneralHelper.SetVisibleColumns(grid, "CategoryName");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "CategoryName", "Category" },
                                });
                    break;
            }
        }
    }
}
