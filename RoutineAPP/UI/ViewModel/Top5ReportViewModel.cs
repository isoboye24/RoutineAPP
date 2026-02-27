using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.UI.ViewModel
{
    public class Top5ReportViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalMinutes { get; set; }
        public string FormattedTotalMinutes { get; set; }
        public string Percentage { get; set; }
    }
}
