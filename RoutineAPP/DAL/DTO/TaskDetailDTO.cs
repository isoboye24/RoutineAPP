using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class TaskDetailDTO
    {
        public int TaskID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int TimeSpent { get; set; }
        public int Day { get; set; }
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public int DailyRoutineID { get; set; }
        public string TimeInHoursAndMinutes { get; set; }
        public string Summary { get; set; }
    }
}
