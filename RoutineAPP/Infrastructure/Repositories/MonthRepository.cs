using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System.Linq;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class MonthRepository : IMonthRepository
    {
        private readonly RoutineDBEntities _db;

        public MonthRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public IQueryable<MONTH> GetAll()
        {
            return _db.MONTHs;
        }
    }
}
