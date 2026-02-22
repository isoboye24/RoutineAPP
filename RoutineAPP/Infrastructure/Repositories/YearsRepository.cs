using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class YearsRepository : IYearsRepository
    {
        private readonly RoutineDBEntities _db;

        public YearsRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public List<int> GetAll()
        {
            return _db.DAILY_ROUTINE
                .Where(x => !x.isDeleted)
                .Select(x => x.routineDate.Year)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();
        }
    }
}
