using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IDashboardService
    {
        string GetCategoryMonthly(int month, int year, string category);
        string GetCategoryAnually(int year, string category);
    }
}
