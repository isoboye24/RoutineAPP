using RoutineAPP.Core.Entities;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IMonthRepository
    {
        List<MonthViewModel> GetAll();
        MonthViewModel GetById(int id);
    }
}
