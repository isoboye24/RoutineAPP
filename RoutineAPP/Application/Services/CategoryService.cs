using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutineAPP.Application.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<CategoryDTO> GetAll()
        {
            return _repository.GetAll()
                .Select(x => new CategoryDTO
                {
                    CategoryID = x.categoryID,
                    CategoryName = x.categoryName
                })
                .OrderBy(x => x.CategoryName)
                .ToList();
        }

        public List<CategoryDTO> GetAllDeletedCategories()
        {
            return _repository.GetAllDeletedCategories()
                .Select(x => new CategoryDTO
                {
                    CategoryID = x.categoryID,
                    CategoryName = x.categoryName
                })
                .OrderBy(x => x.CategoryName)
                .ToList();
        }
        
        public bool Create(Category category)
        {
            if (_repository.Exists(category.Name))
                throw new Exception("Category already exists");

            return _repository.Insert(category);
        }

        public bool Update(Category category)
        {
            var check = _repository.GetById(category.Id);
            if (check == null)
                throw new Exception("Category not found");

            category.UpdateName(category.Name);
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
