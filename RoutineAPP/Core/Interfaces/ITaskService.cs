using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ITaskService
    {
        List<Entities.Task> GetAll();
        bool Create(int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary);
        bool Update(int id, int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
    }
}
