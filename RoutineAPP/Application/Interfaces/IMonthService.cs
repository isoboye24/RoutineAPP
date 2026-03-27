using RoutineAPP.Core.Entities;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface IMonthService
    {
        List<MonthDTO> GetAll();
    }
}
