using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly FoodsService _foodsService;

        public FoodsController(ApplicationContext context, FoodsService foodsService)
        {
            _context = context;
            _foodsService = foodsService;
        }

        // GET: api/Foods
        [HttpGet]
        public ActionResult<IEnumerable<FoodResponse>> GetFoods()
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            return _foodsService.GetFoodsResponses();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public ActionResult<FoodResponse> GetFood(int id)
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            var food = _context.Foods.Find(id);

            if (food == null)
            {
                return NotFound();
            }
            return _foodsService.GetFoodResponse(food);
        }

        // PUT: api/Foods/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<FoodResponse> PutFood(int id, FoodCreate updatedFood)
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            var existingFood = _context.Foods.Find(id);
            if (existingFood == null)
            {
                return BadRequest();
            }


            return _foodsService.UpdateFood(existingFood, updatedFood);
        }

        // POST: api/Foods
        [HttpPost]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<int> PostFood(FoodCreate foodCreate)
        {
            if (_context.Foods == null)
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            if (_context.States == null)
            {
                return Problem("Entity set 'ApplicationContext.States'  is null.");
            }
            return _foodsService.CreateFood(foodCreate);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult DeleteFood(int id)
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            var food = _context.Foods.Find(id);
            if (food == null)
            {
                return NotFound();
            }

            _foodsService.DeleteFoodAndRelatedEntities(food);

            return Ok();
        }

    }
}
