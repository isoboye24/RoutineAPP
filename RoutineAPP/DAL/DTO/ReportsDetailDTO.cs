using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class ReportsDetailDTO
    {
        public int ReportID { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string TotalTimeUsed { get; set; }
        public string PercentageOfUsedTime { get; set; }
        public double TotalTimeForFormatting { get; set; }
        public int Year { get; set; }
    }
}
