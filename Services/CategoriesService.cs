using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Qr_Menu_API.Data;
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

        public CategoriesService(ApplicationContext context, FoodsService foodsService)
        {
            _context = context;
            _foodsService = foodsService;
        }
        public CategoryResponse GetCategoryResponse(Category category)
        {
            CategoryResponse categoryResponse = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                RestaurantId = category.RestaurantId,
                StateResponse = new StateResponse
                {
                    Id = category.StateId,
                    Name = _context.States!.Find(category.StateId)!.Name
                },
                FoodIds = category.Foods?.Select(f => f.Id).ToList()
            };
            return categoryResponse;
        }

        public List<CategoryResponse> GetCategoriesResponses()
        {
            List<Category> categories = _context.Categories.Include(c => c.Foods).ToList();
            List<CategoryResponse> categoriesResponses = new();
            categories.ForEach(category =>
            {
                categoriesResponses.Add(GetCategoryResponse(category));
            });
            return categoriesResponses;
        }

        public int CreateCategory(CategoryCreate categoryCreate)
        {
            Category newCategory = new()
            {
                Name = categoryCreate.Name,
                Description = categoryCreate.Description,
                RestaurantId = categoryCreate.RestaurantId,
                StateId = (byte)1,
                Foods = new List<Food>()
            };

            _context.Categories.Add(newCategory);
            var restaurant = _context.Restaurants.Include(r => r.Categories).FirstOrDefault(r => r.Id == categoryCreate.RestaurantId);
            if (restaurant != null)
            {
                restaurant.Categories.Add(newCategory);
            }
            _context.SaveChanges();
            return newCategory.Id;
        }

        public CategoryResponse UpdateCategory(Category existingCategory, CategoryCreate updatedCategory)
        {
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;
            _context.Update(existingCategory);
            _context.SaveChanges();
            return GetCategoryResponse(existingCategory);
        }

        public void DeleteCategoryAndRelatedEntities(Category category)
        {
            category.StateId = 0;
            _context.Categories.Update(category);
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

        public CategoryDetailedResponse GetDetailedCategoryResponse(Category category)
        {
            CategoryDetailedResponse categoryDetailedResponse = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                RestaurantId = category.RestaurantId,
                StateResponse = new StateResponse
                {
                    Id = category.StateId,
                    Name = _context.States!.Find(category.StateId)!.Name
                },
                FoodsDetailed = new List<FoodResponse>()
            };
            if (category.Foods != null)
            {
                foreach (Food food in category.Foods)
                {
                    FoodResponse foodDetailedResponse = _foodsService.GetFoodResponse(food);
                    categoryDetailedResponse.FoodsDetailed.Add(foodDetailedResponse);
                }
            }
            return categoryDetailedResponse;
        }
    }
}
