using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class DailyTaskDTO
    {
        public List<DailyTaskDetailDTO> DailyRoutines { get; set; }
        public List<MonthDetailDTO> Months { get; set; }
    }
}
