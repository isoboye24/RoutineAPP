using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        bool Create(string name);
        bool Update(int id, string name);
        bool Delete(int id);
        int Count();
    }
}
