using RoutineAPP.Core.Entities;
using RoutineAPP.Application.DTO;
using System.Collections.Generic;

namespace RoutineAPP.Application.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetAll();
        List<CategoryDTO> GetAllDeletedCategories();
        bool Create(Category category);
        bool Update(Category category);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
    }
}
