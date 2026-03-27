using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface ITaskRepository
    {
        List<Core.Entities.Task> GetAll(int routineId);
        Core.Entities.Task GetById(int id);
        bool Insert(Core.Entities.Task routine);
        bool Update(Core.Entities.Task routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(int categoryId, int routineId);
        int Count();
        List<TaskDTO> GetTaskDetails(int dailyId);
        List<TaskDTO> GetTasksByDay(int routine);
        List<TaskDTO> GetTasksByMonth(int month, int year);
        List<TaskDTO> GetTasksByYear(int year);
        List<TaskDTO> GetTotalTasks();

        List<Top5ReportDTO> GetTop5MonthlyReport(int month, int year);
        List<Top5ReportDTO> GetTop5AnnualReport(int year);

        int GetCategoryTimeMonthly(int month, int year, string category);
        int GetCategoryTimeAnually(int year, string category);
    }
}
