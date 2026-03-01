using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class MonthRepository : IMonthRepository
    {
        private readonly RoutineDBEntities _db;

        public MonthRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public List<MonthViewModel> GetAll()
        {
            return _db.MONTHs
                .OrderBy(x => x.monthID)
                .Select(x => new MonthViewModel
                {
                    MonthID = x.monthID,
                    MonthName = x.monthName
                })
                .ToList();
        }

        public MonthViewModel GetById(int id)
        {
            var entity = _db.MONTHs.FirstOrDefault(x => x.monthID == id);
            if (entity == null) return null;

            else
            {
                List<MonthViewModel> list = new List<MonthViewModel>();
                list.Add(new MonthViewModel
                {
                    MonthID = entity.monthID,
                    MonthName = entity.monthName
                });

                return list.FirstOrDefault();
            }
        }
    }
}
