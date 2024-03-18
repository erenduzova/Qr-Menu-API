using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class RestaurantsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantsService _restaurantsService;

        public RestaurantsController(ApplicationContext context, RestaurantsService restaurantsService)
        {
            _context = context;
            _restaurantsService = restaurantsService;
        }

        // GET: api/Restaurants
        [HttpGet]
        public ActionResult<IEnumerable<RestaurantResponse>> GetRestaurants()
        {
          if (_context.Restaurants == null)
          {
              return NotFound();
          }
            return _restaurantsService.GetRestaurantsResponses();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public ActionResult<RestaurantResponse> GetRestaurant(int id)
        {
          if (_context.Restaurants == null)
          {
              return NotFound();
          }
            var restaurant = _context.Restaurants.Include(r => r.Categories).FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return _restaurantsService.GetRestaurantResponse(restaurant);
        }

        // PUT: api/Restaurants/5
        [HttpPut("{id}")]
        public ActionResult<RestaurantResponse> PutRestaurant(int id, RestaurantCreate updatedRestaurant)
        {
            var existingRestaurant = _context.Restaurants.Include(r => r.Categories).FirstOrDefault(r => r.Id == id);
            if (existingRestaurant == null)
            {
                return BadRequest();
            }
            return _restaurantsService.UpdateRestaurant(existingRestaurant, updatedRestaurant);
        }

        // POST: api/Restaurants
        [HttpPost]
        public ActionResult<int> PostRestaurant(RestaurantCreate restaurantCreate)
        {
            if (_context.Restaurants == null)
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            if (_context.States == null)
            {
                return Problem("Entity set 'ApplicationContext.States'  is null.");
            }
            return _restaurantsService.CreateRestaurant(restaurantCreate);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant(int id)
        {
            var restaurant = _context.Restaurants.Include(r => r.Categories).FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _restaurantsService.DeleteRestaurantAndRelatedEntities(restaurant);

            //
            //
            // RestaurantUsers  edit to deleted
            //
            //

            return Ok();
        }

        [HttpGet("Detailed/{id}")]
        public ActionResult<RestaurantDetailedResponse> GetDetailedRestaurant(int id)
        {
            if (_context.Restaurants == null)
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            var restaurant = _context.Restaurants.Include(r => r.Categories)
                .ThenInclude(c => c.Foods)
                .FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return _restaurantsService.GetRestaurantDetailedResponse(restaurant);
        }

    }
}
