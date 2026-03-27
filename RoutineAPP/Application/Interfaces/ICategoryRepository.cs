using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using System.Linq;

namespace RoutineAPP.Application.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<CATEGORY> GetAll();
        IQueryable<CATEGORY> GetAllDeletedCategories();
        IQueryable<CATEGORY> GetById(int id);
        bool Insert(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(string name);
        int Count();
    }
}
