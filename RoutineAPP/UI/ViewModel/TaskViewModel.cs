using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.UI.ViewModel
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public int DailyRoutineId { get; set; }
        public DateTime DailyRoutineDate { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int TimeSpent { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public string Summary { get; set; }
        public string TimeInHoursAndMinutes { get; set; }
    }
}
