using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.HelperService
{
    public class TaskHelperService
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
                    GeneralHelperService.SetVisibleColumns(grid, "CategoryName", "TimeSpent", "TimeInHoursAndMinutes");
                    GeneralHelperService.RenameColumns(grid, new Dictionary<string, string>
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
