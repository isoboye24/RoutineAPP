using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DTO
{
    public class GraphDTO
    {
        public List<GraphDetailDTO> Graphs { get; set; }
        public List<MonthDetailDTO> Months { get; set; }
        public List<CategoryDetailDTO> Categories { get; set; }
    }
}
