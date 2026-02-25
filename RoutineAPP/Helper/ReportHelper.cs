using RoutineAPP.HelperService;
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
        }

        public static void ConfigureGetAllMonthsGrid(DataGridView grid, ReportGridType type)
        {
            switch (type)
            {
                case ReportGridType.GetAllMonths:
                    GeneralHelper.SetVisibleColumns(grid, "Month", "Year");                    
                    break;
            }
        }
        public static void ConfigureReportDetailsGrid(DataGridView grid, ReportGridType type)
        {
            switch (type)
            {
                case ReportGridType.ReportDetails:
                    GeneralHelper.SetVisibleColumns(grid, "Category", "TotalTimeUsed", "PercentageOfUsedTime", "TotalTimeForFormatting");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "CategoryName", "Category" },
                                    { "TotalTimeUsed", "In Minute" },
                                    { "PercentageOfUsedTime", "% in 2 dp" },
                                    { "TotalTimeForFormatting", "Complete %" },
                                });
                    break;
            }
        }

    }
}
