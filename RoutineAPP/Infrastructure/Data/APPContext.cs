using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DAO
{
    public class APPContext
    {
        public RoutineDBEntities db = new RoutineDBEntities();
    }
}
