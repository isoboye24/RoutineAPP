using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.DTO
{
    public class ReportDTO
    {
        public int ReportID { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string TotalTimeUsed { get; set; }
        public string PercentageOfUsedTime { get; set; }
        public double CompletePercentage { get; set; }
        public int Year { get; set; }
    }
}
