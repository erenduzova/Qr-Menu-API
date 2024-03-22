using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.DetailedResponseDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;

namespace Qr_Menu_API.DTOs.Converter
{
	public class CategoryConverter
	{
        private readonly StateConverter _stateConverter;
        private FoodConverter _foodConverter;

		public CategoryConverter(StateConverter stateConverter, FoodConverter foodConverter)
        {
            _stateConverter = stateConverter;
            _foodConverter = foodConverter;
		}

        public Category Convert(CategoryCreate categoryCreate)
        {
            Category newCategory = new()
            {
                Name = categoryCreate.Name,
                Description = categoryCreate.Description,
                RestaurantId = categoryCreate.RestaurantId,
                StateId = (byte)1,
                Foods = new List<Food>()
            };
            return newCategory;
        }

        public CategoryResponse Convert(Category category)
        {
            CategoryResponse categoryResponse = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                RestaurantId = category.RestaurantId,
                StateResponse = _stateConverter.Convert(category.State!),
                FoodIds = category.Foods?.Select(f => f.Id).ToList()
            };
            return categoryResponse;
        }

        public List<CategoryResponse> Convert(List<Category> categories)
        {
            List<CategoryResponse> categoryResponses = new List<CategoryResponse>();
            foreach (Category category in categories)
            {
                categoryResponses.Add(Convert(category));
            }
            return categoryResponses;
        }

        public Category Convert(Category existingCategory, CategoryCreate updatedCategory)
        {
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;
            return existingCategory;
        }

        public CategoryDetailedResponse ConvertDetailed(Category category)
        {
            CategoryDetailedResponse categoryDetailedResponse = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                RestaurantId = category.RestaurantId,
                StateResponse = _stateConverter.Convert(category.State!),
                FoodsDetailed = new List<FoodResponse>()
            };
            if (category.Foods != null)
            {
                foreach (Food food in category.Foods)
                {
                    FoodResponse foodDetailedResponse = _foodConverter.Convert(food);
                    categoryDetailedResponse.FoodsDetailed.Add(foodDetailedResponse);
                }
            }
            return categoryDetailedResponse;
        }

    }
}

