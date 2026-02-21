using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IMonthRepository
    {
        List<Month> GetAll();
        Month GetById(int id);
    }
}
