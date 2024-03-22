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

        private bool FoodsIsNull()
        {
            return _context.Foods == null;
        }

        private bool FoodExists(int id)
        {
            return _context.Foods!
                .Any(c => c.Id == id);
        }

        // GET: api/Foods
        [HttpGet]
        public ActionResult<IEnumerable<FoodResponse>> GetFoods()
        {
            if (FoodsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            return _foodsService.GetFoodsResponses();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public ActionResult<FoodResponse> GetFood(int id)
        {
            if (FoodsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            if (!FoodExists(id))
            {
                return NotFound("Food not found with this id: " + id);
            }
            return _foodsService.GetFoodResponse(id);
        }

        // PUT: api/Foods/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<FoodResponse> PutFood(int id, FoodCreate updatedFood)
        {
            if (FoodsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            if (!FoodExists(id))
            {
                return NotFound("Food not found with this id: " + id);
            }
            return _foodsService.UpdateFood(id, updatedFood);
        }

        // POST: api/Foods
        [HttpPost]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<int> PostFood(FoodCreate foodCreate)
        {
            if (FoodsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            return _foodsService.CreateFood(foodCreate);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult DeleteFood(int id)
        {
            if (FoodsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Foods'  is null.");
            }
            if (!FoodExists(id))
            {
                return NotFound("Food not found with this id: " + id);
            }
            _foodsService.DeleteFoodAndRelatedEntitiesById(id);
            return Ok();
        }

    }
}
