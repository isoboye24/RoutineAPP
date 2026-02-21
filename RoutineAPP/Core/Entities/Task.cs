using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Entities
{
    public class Task
    {
        public int Id { get; private set; }
        public int DailyRoutineId { get; private set; }
        public int CategoryId { get; private set; }
        public int TimeSpent { get; private set; }
        public int Day { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public string Summary { get; private set; }

        private Task() { }

        public Task(int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary = null)
        {
            DailyRoutineId = dailyRoutineId;
            CategoryId = categoryId;
            TimeSpent = timeSpent;
            Day = day;
            Month = month;
            Year = year;

            Summary = string.IsNullOrWhiteSpace(summary)
                ? null
                : summary.Trim();
        }

        public static Task Rehydrate(
            int id,
            int dailyRoutineId, 
            int categoryId,
            int timeSpent,
            int day, 
            int month, 
            int year,
            string summary)
        {
            return new Task(dailyRoutineId, categoryId, timeSpent, day, month, year, summary)
            {
                Id = id
            };
        }

        public void Update(int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary)
        {
            DailyRoutineId = dailyRoutineId;
            CategoryId = categoryId;
            TimeSpent = timeSpent;
            Day = day;
            Month = month;
            Year = year;

            Summary = string.IsNullOrWhiteSpace(summary)
                ? null
                : summary.Trim();
        }

        public void SetId(int id)
        {
            Id = id;
        }

    }
}
