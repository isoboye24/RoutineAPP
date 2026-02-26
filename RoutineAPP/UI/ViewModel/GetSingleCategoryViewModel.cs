using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.UI.ViewModel
{
    public class GetSingleCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MonthID { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int TotalHours { get; set; }
    }
}
