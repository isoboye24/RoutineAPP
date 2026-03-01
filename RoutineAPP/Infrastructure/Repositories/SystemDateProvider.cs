using RoutineAPP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class SystemDateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
    }
}
