using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(int id);
        bool Insert(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool Exists(string name);
        int Count();
    }
}
