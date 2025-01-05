using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class ReportDTO
    {
        public List<MonthlyRoutinesDetailDTO> MonthlyRoutineReports { get; set; }
        public List<ReportsDetailDTO> MonthlyReports { get; set; }
        public List<YearDetailDTO> YearlyRoutineReports { get; set; }
        public List<ReportsDetailDTO> YearlyReports { get; set; }
        public List<ReportsDetailDTO> TotalReports { get; set; }
        public List<MonthDetailDTO> Months { get; set; }
        public List<CategoryDetailDTO> Categories { get; set; }
    }
}
