using System;


namespace RoutineAPP.Core.Entities
{
    public class Task
    {
        public int Id { get; private set; }
        public int DailyRoutineId { get; private set; }
        public int CategoryId { get; private set; }
        public int TimeSpent { get; private set; }
        public string Summary { get; private set; }
        public DateTime TaskDate { get; private set; }

        public Task(int dailyRoutineId, int categoryId, int timeSpent, DateTime date, string summary = null)
        {
            SetDailyRoutineId(dailyRoutineId);
            SetCategoryId(categoryId);
            SetTimeSpent(timeSpent);
            SetTaskDate(date);
            SetSummary(summary);
        }

        public static Task Rehydrate(
            int id,
            int dailyRoutineId, 
            int categoryId,
            int timeSpent,
            DateTime date,
            string summary)
        {
            var task = new Task(dailyRoutineId, categoryId, timeSpent, date, summary);
            task.Id = id;
            return task;
        }

        private void SetDailyRoutineId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Invalid Daily routine");

            DailyRoutineId = id;
        }

        public void UpdateCategory(int newId)
        {
            SetCategoryId(newId);
        }

        private void SetCategoryId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Invalid Category");

            CategoryId = id;
        }

        public void UpdateTimeSpent(int newTimeSpent)
        {
            SetTimeSpent(newTimeSpent);
        }

        private void SetTimeSpent(int timeSpent)
        {
            if (timeSpent < 0)
                throw new ArgumentException("Time must be an integer and more than 0");

            TimeSpent = timeSpent;
        }

        public void UpdateTaskDate(DateTime newDate)
        {
            SetTaskDate(newDate);
        }

        private void SetTaskDate(DateTime date)
        {
            if (date.Year < 2024 || date.Year > DateTime.Now.Year + 1)
                throw new ArgumentException("Invalid year");

            TaskDate = date;
        }

        public void UpdateSummary(string newSummary)
        {
            SetSummary(newSummary);
        }
        
        private void SetSummary(string summary)
        {
            Summary = string.IsNullOrWhiteSpace(summary) ? null : summary.Trim();
        }
    }
}
