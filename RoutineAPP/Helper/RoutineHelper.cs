using System.Collections.Generic;
using System.Windows.Forms;

namespace RoutineAPP.Helper
{
    public class RoutineHelper
    {
        public enum RoutineGridType
        {
            Basic,
        }

        public static void ConfigureDailyRoutineGrid(DataGridView grid, RoutineGridType type)
        {
            switch (type)
            {
                case RoutineGridType.Basic:
                    GeneralHelper.SetVisibleColumns(grid, "Day", "MonthName", "Year");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "MonthName", "Month" },                                    
                                });
                    break;
            }
        }
    }
}
