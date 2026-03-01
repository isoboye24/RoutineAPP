using RoutineAPP.Core.Entities;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ICategoryRepository
    {
        List<CategoryViewModel> GetAll();
        Category GetById(int id);
        bool Insert(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(string name);
        int Count();
    }
}
