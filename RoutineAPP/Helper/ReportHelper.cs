using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.Helper
{
    public class ReportHelper
    {
        public enum ReportGridType
        {
            GetAllMonths,
            ReportDetails,
            Top5ReportDetails,
        }

        public static void ConfigureReportDetailsGrid(DataGridView grid, ReportGridType type)
        {
            switch (type)
            {
                case ReportGridType.GetAllMonths:
                    GeneralHelper.SetVisibleColumns(grid, "Month", "Year");
                    break;

                case ReportGridType.ReportDetails:
                    GeneralHelper.SetVisibleColumns(grid, "Category", "TotalTimeUsed", "PercentageOfUsedTime");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "CategoryName", "Category" },
                                    { "TotalTimeUsed", "In Minute" },
                                    { "PercentageOfUsedTime", "%" },
                                });
                    break;

                case ReportGridType.Top5ReportDetails:
                    GeneralHelper.SetVisibleColumns(grid, "CategoryName", "FormattedTotalMinutes", "Percentage");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "CategoryName", "Cat" },
                                    { "FormattedTotalMinutes", "Time" },
                                    { "Percentage", "% " },
                                });
                    break;
            }
        }

    }
}
