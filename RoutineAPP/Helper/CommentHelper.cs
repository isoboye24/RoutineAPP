using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.Helper
{
    public class CommentHelper
    {
        public enum CommentGridType
        {
            Basic,
        }

        public static void ConfigureCommentGrid(DataGridView grid, CommentGridType type)
        {
            switch (type)
            {
                case CommentGridType.Basic:
                    GeneralHelper.SetVisibleColumns(grid, "Summary",  "Day", "MonthName", "Year");
                    GeneralHelper.RenameColumns(grid, new Dictionary<string, string>
                                {
                                    { "MonthName", "Month" },
                                });
                    break;
            }
        }
    }
}
