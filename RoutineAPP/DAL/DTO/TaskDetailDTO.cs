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
        public decimal TimeSpent { get; set; }
        public int Day { get; set; }
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
    }
}
