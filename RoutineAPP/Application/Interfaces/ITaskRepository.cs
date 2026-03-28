using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using System.Linq;

namespace RoutineAPP.Application.Interfaces
{
    public interface ITaskRepository
    {
        IQueryable<TASK> GetAll();
        IQueryable<TASK> GetAllDeletedTasks();
        IQueryable<TASK> GetTasksByDay(int routineId);
        IQueryable<TASK> GetById(int id);
        IQueryable<TASK> GetTasksByMonth(int month, int year);
        IQueryable<TASK> GetTasksByYear(int year);

        bool Insert(Task routine);
        bool Update(Task routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(int categoryId, int routineId);
        int Count();

        int GetCategoryTimeMonthly(int month, int year, string category);
        int GetCategoryTimeAnually(int year, string category);
    }
}
