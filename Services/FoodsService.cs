using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.Services
{
	public class FoodsService
	{
        private readonly ApplicationContext _context;
        private readonly FoodConverter _foodConverter;

		public FoodsService(ApplicationContext context, FoodConverter foodConverter)
		{
			_context = context;
            _foodConverter = foodConverter;
		}

        private Food GetFood(int foodId)
        {
            return _context.Foods!
                .Include(c => c.State)
                .First(c => c.Id == foodId);
        }

        private List<Food> GetFoods()
        {
            return _context.Foods!
                .Include(c => c.State)
                .ToList();
        }

        private Food GetFoodWithCategory(int foodId)
        {
            return _context.Foods!
                .Include(c => c.State)
                .Include(c => c.Category)
                .First(c => c.Id == foodId);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories!
                .Any(c => c.Id == id && c.StateId != 0);
        }

        public FoodResponse GetFoodResponse(int id)
        {
            Food foundFood = GetFood(id);
            return _foodConverter.Convert(foundFood);
        }

        public List<FoodResponse> GetFoodsResponses()
        {
            List<Food> foods = GetFoods();
            return _foodConverter.Convert(foods);
        }

        public int CreateFood(FoodCreate foodCreate)
        {
            if (!CategoryExists(foodCreate.CategoryId))
            {
                return -1;
            }
            Food newFood = _foodConverter.Convert(foodCreate);
            _context.Foods!.Add(newFood);
            _context.SaveChanges();
            return newFood.Id;
        }

        public FoodResponse UpdateFood(int foodId, FoodCreate updatedFood)
        {
            Food existingFood = GetFood(foodId);
            existingFood = _foodConverter.Convert(existingFood, updatedFood);
            _context.Update(existingFood);
            _context.SaveChanges();
            return _foodConverter.Convert(existingFood);
        }

        public void DeleteFoodAndRelatedEntities(Food food)
        {
            food.StateId = 0;
            _context.Foods!.Update(food);
            _context.SaveChanges();
        }

        public void DeleteFoodAndRelatedEntitiesById(int id)
        {
            Food food = GetFoodWithCategory(id);
            DeleteFoodAndRelatedEntities(food);
        }

        public string uploadFoodImage(int id, IFormFile imageFile)
        {
            Food food = _context.Foods!.First(f => f.Id == id);

            // Get image extension
            string fileExtension = Path.GetExtension(imageFile.FileName);
            // Create unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            // Image save path
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", uniqueFileName);

            // Create Image file if not exist
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Image")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Image"));
            }

            // Save File
            using (FileStream fileStream = new(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            food.ImageUrl = imagePath;
            _context.SaveChanges();

            // Return path of the image
            return imagePath;
        }
    }
}

