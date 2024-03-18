using System;
using Microsoft.AspNetCore.Mvc;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.Services
{
	public class FoodsService
	{
        private readonly ApplicationContext _context;

		public FoodsService(ApplicationContext context)
		{
			_context = context;
		}

        public FoodResponse GetFoodResponse(Food food)
        {
            FoodResponse foodResponse = new()
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                Description= food.Description,
                CategoryId = food.CategoryId,
                StateResponse = new StateResponse
                {
                    Id = food.StateId,
                    Name = _context.States!.Find(food.StateId)!.Name
                }
            };
            return foodResponse;
        }

        public List<FoodResponse> GetFoodsResponses()
        {
            List<Food> foods = _context.Foods!.ToList();
            List<FoodResponse> foodResponses = new();
            foods.ForEach(food =>
            {
                foodResponses.Add(GetFoodResponse(food));
            });
            return foodResponses;
        }

        public int CreateFood(FoodCreate foodCreate)
        {
            Food newFood = new()
            {
                Name = foodCreate.Name,
                Price = foodCreate.Price,
                Description = foodCreate.Description,
                CategoryId = foodCreate.CategoryId,
                StateId = (byte)1
            };

            _context.Foods.Add(newFood);
            _context.SaveChanges();

            return newFood.Id;
        }

        public FoodResponse UpdateFood(Food existingFood, FoodCreate updatedFood)
        {
            existingFood.Name = updatedFood.Name;
            existingFood.Price = updatedFood.Price;
            existingFood.Description = updatedFood.Description;

            _context.Update(existingFood);
            _context.SaveChanges();
            return GetFoodResponse(existingFood);
        }

        public void DeleteFoodAndRelatedEntities(Food food)
        {
            food.StateId = 0;
            _context.Foods.Update(food);
            _context.SaveChanges();
        }
    }
}

