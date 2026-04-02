using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Linq;

namespace RoutineAPP.Application.Interfaces
{
    public interface IDailyRoutineRepository
    {
        IQueryable<DAILY_ROUTINE> GetAll();
        IQueryable<DAILY_ROUTINE> GetAllByYear(int year);
        IQueryable<DAILY_ROUTINE> GetAllByMonth(int month, int year);
        IQueryable<DAILY_ROUTINE> GetAllDeletedRoutines();
        IQueryable<DAILY_ROUTINE> GetById(int id);
        bool Insert(DailyRoutine routine);
        bool Update(DailyRoutine routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(DateTime date);
        int Count();

        int CountByMonth(int month, int year);
        int CountByYear(int year);

        IQueryable<DAILY_ROUTINE> GetComments(int year);
        IQueryable<DAILY_ROUTINE> GetAllComments();
        IQueryable<DAILY_ROUTINE> GetCommentById(int id);

        IQueryable<int> GetYears();

        int GetSummaryCount();
        int GetSummaryCountByYear(int year);
    }
}
