using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Entities
{
    public class DailyRoutine
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public string Summary { get; private set; }

        public DailyRoutine(DateTime date, string summary = null)
        {
            Date = date;

            Summary = string.IsNullOrWhiteSpace(summary)
                ? null
                : summary.Trim();
        }

        public static DailyRoutine Rehydrate(
            int id,
            DateTime date,
            string summary)
        {
            return new DailyRoutine(date, summary)
            {
                Id = id
            };
        }

        public void Update(DateTime date, string summary)
        {
            Date = date;

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
