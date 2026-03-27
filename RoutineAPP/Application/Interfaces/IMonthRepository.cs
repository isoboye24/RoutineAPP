using RoutineAPP.Application.DTO;
using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface IMonthRepository
    {
        IQueryable<MONTH> GetAll();
    }
}
