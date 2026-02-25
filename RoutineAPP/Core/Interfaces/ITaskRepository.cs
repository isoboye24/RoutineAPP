using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ITaskRepository
    {
        List<Entities.Task> GetAll(int routineId);
        Entities.Task GetById(int id);
        bool Insert(Entities.Task routine);
        bool Update(Entities.Task routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(int year, int month, int day);
        int Count();
        List<TaskViewModel> GetTaskDetails(int dailyId);
        List<TaskViewModel> GetTasksByDay(int routine);
        List<TaskViewModel> GetTasksByMonth(int month, int year);
        List<TaskViewModel> GetTasksByYear(int year);
        List<TaskViewModel> GetTotalTasks();
    }
}
