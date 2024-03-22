using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.DetailedResponseDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Services
{
    public class CategoriesService
    {
        private readonly ApplicationContext _context;
        private readonly FoodsService _foodsService;
        private readonly CategoryConverter _categoryConverter;

        public CategoriesService(ApplicationContext context, FoodsService foodsService, CategoryConverter categoryConverter)
        {
            _context = context;
            _foodsService = foodsService;
            _categoryConverter = categoryConverter;
        }

        private Category GetCategory(int categoryId)
        {
            return _context.Categories!
                .Include(c => c.State)
                .First(c => c.Id == categoryId);
        }

        private List<Category> GetCategories()
        {
            return _context.Categories!
                .Include(c => c.State)
                .ToList();
        }

        private Category GetCategoryWithFoods(int categoryId)
        {
            return _context.Categories!
                .Include(c => c.State)
                .Include(c => c.Foods)
                .First(c => c.Id == categoryId);
        }

        private List<Category> GetCategoriesWithFoods()
        {
            return _context.Categories!
                .Include(c => c.State)
                .Include(c => c.Foods)
                .ToList();
        }

        public CategoryResponse GetCategoryResponse(int id)
        {
            Category foundCategory = GetCategoryWithFoods(id);
            return _categoryConverter.Convert(foundCategory);
        }

        public List<CategoryResponse> GetCategoriesResponses()
        {
            List<Category> categories = GetCategoriesWithFoods();
            return _categoryConverter.Convert(categories);
        }

        public int CreateCategory(CategoryCreate categoryCreate)
        {
            Category newCategory = _categoryConverter.Convert(categoryCreate);
            _context.Categories!.Add(newCategory);
            _context.SaveChanges();
            return newCategory.Id;
        }

        public CategoryResponse UpdateCategory(int id, CategoryCreate updatedCategory)
        {
            Category existingCategory = GetCategoryWithFoods(id);
            existingCategory = _categoryConverter.Convert(existingCategory, updatedCategory);
            _context.Update(existingCategory);
            _context.SaveChanges();
            return _categoryConverter.Convert(existingCategory);
        }

        public void DeleteCategoryAndRelatedEntities(Category category)
        {
            category.StateId = 0;
            _context.Categories!.Update(category);
            var foods = category.Foods;
            if (foods != null)
            {
                foreach (var food in foods)
                {
                    _foodsService.DeleteFoodAndRelatedEntities(food);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteCategoryAndRelatedEntitiesById(int id)
        {
            Category category = GetCategoryWithFoods(id);
            DeleteCategoryAndRelatedEntities(category);
        }

        public CategoryDetailedResponse GetDetailedCategoryResponse(int id)
        {
            Category category = GetCategoryWithFoods(id);
            return _categoryConverter.ConvertDetailed(category);
        }
    }
}
