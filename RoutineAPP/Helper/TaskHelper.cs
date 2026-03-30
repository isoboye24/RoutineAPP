using System.Collections.Generic;
using System.Windows.Forms;

namespace RoutineAPP.Helper
{
    public class TaskHelper
    {
        public enum TaskGridType
        {
            Basic
        }

        public static void ConfigureTaskGrid(DataGridView grid, TaskGridType type)
        {
            switch (type)
            {
                case TaskGridType.Basic:
                    GeneralHelper.SetVisibleColumns(grid, "Category", "TimeSpent", "TimeInHoursAndMinutes");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "CategoryName", "Category" },
                                    { "TimeSpent", "In Minute" },
                                    { "TimeInHoursAndMinutes", "In Hour" },
                                });
                    break;
            }
        }
    }
}
