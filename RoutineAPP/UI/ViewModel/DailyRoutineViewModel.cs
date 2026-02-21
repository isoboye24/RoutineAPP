using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.UI.ViewModel
{
    public class DailyRoutineViewModel
    {
        public int Id { get; set; }
        public DateTime RoutineDate { get; set; }
        public string Summary { get; set; }
        public int Day { get; set; }
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
    }
}
