using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<CategoryViewModel> GetAll()
            => _repository.GetAll();

        public bool Create(string name)
        {
            if (_repository.Exists(name))
                throw new Exception("Category already exists");

            var category = new Category(name);
            return _repository.Insert(category);
        }

        public bool Update(int id, string name)
        {
            var category = _repository.GetById(id);
            if (category == null)
                throw new Exception("Category not found");

            category.UpdateName(name);
            return _repository.Update(category);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public bool PermanentDelete(int id)
            => _repository.PermanentDelete(id);

        public int Count()
            => _repository.Count();
    }
}
