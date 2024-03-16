using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class TaskDTO
    {
        public List<MonthDetailDTO> Months { get; set; }
        public List<CategoryDetailDTO> Categories { get; set; }
        public List<TaskDetailDTO> Tasks { get; set; }
    }
}
