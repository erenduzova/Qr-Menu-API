using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.DTOs.Converter
{
	public class FoodConverter
	{
        private readonly StateConverter _stateConverter;

        public FoodConverter(StateConverter stateConverter)
        {
            _stateConverter = stateConverter;
        }

        public Food Convert(FoodCreate foodCreate)
        {
            Food newFood = new()
            {
                Name = foodCreate.Name,
                Price = foodCreate.Price,
                Description = foodCreate.Description,
                CategoryId = foodCreate.CategoryId,
                StateId = (byte)1
            };
            return newFood;
        }

        public FoodResponse Convert(Food food)
        {
            FoodResponse foodResponse = new()
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                Description = food.Description,
                CategoryId = food.CategoryId,
                StateResponse = _stateConverter.Convert(food.State!),
            };
            return foodResponse;
        }

        public List<FoodResponse> Convert(List<Food> foods)
        {
            List<FoodResponse> foodResponses = new List<FoodResponse>();
            foreach (Food food in foods)
            {
                foodResponses.Add(Convert(food));
            }
            return foodResponses;
        }

        public Food Convert(Food existingFood, FoodCreate updatedFood)
        {
            existingFood.Name = updatedFood.Name;
            existingFood.Price = updatedFood.Price;
            existingFood.Description = updatedFood.Description;
            return existingFood;
        }
    }
}

