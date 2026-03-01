using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IDashboardService
    {
        string GetCategoryTimeMonthly(int month, int year, string category);
        string GetCategoryTimeAnually(int year, string category);
    }
}
