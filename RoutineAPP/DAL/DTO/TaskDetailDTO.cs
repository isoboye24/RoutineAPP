using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class TaskDetailDTO
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public int TimeSpent { get; set; }
        public DateTime TaskDate { get; set; }
        public int DailyRoutineID { get; set; }
        public string Summary { get; set; }
    }
}
