using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class MonthRepository
    {
        private readonly RoutineDBEntities _db;

        public MonthRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public List<Month> GetAll()
        {
            return _db.MONTHs
                .OrderBy(x => x.monthID)
                .ToList()
                .Select(x => Month.Rehydrate(x.monthID, x.monthName))
                .ToList();
        }

        public Month GetById(int id)
        {
            var entity = _db.MONTHs.FirstOrDefault(x => x.monthID == id);
            if (entity == null) return null;

            return Month.Rehydrate(entity.monthID, entity.monthName);
        }
    }
}
