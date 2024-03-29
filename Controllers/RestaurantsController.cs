﻿using System;
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
    public class RestaurantsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantsService _restaurantsService;

        public RestaurantsController(ApplicationContext context, RestaurantsService restaurantsService)
        {
            _context = context;
            _restaurantsService = restaurantsService;
        }

        private bool RestaurantsIsNull()
        {
            return _context.Restaurants == null;
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants!
                .Any(r => r.Id == id);
        }

        // GET: api/Restaurants
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<RestaurantResponse>> GetRestaurants()
        {
            if (RestaurantsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            return _restaurantsService.GetRestaurantsResponses();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RestaurantResponse> GetRestaurant(int id)
        {
            if (RestaurantsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }

            return _restaurantsService.GetRestaurantResponse(id);
        }

        // PUT: api/Restaurants/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        [Authorize(Policy = "RestaurantAdministrator")]
        public ActionResult<RestaurantResponse> PutRestaurant(int id, RestaurantCreate updatedRestaurant)
        {
            if (User.HasClaim("RestaurantId", id.ToString()) == false)
            {
                return Unauthorized();
            }
            if (RestaurantsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }
            return _restaurantsService.UpdateRestaurant(id, updatedRestaurant);
        }

        // POST: api/Restaurants
        [HttpPost]
        [Authorize(Roles = "CompanyAdministrator")]
        [Authorize(Policy = "CompanyAdministrator")]
        public ActionResult<int> PostRestaurant(RestaurantCreate restaurantCreate)
        {
            if (User.HasClaim("CompanyId", restaurantCreate.CompanyId.ToString()) == false)
            {
                return Unauthorized();
            }
            if (RestaurantsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            int newRestaurantId = _restaurantsService.CreateRestaurant(restaurantCreate);
            if (newRestaurantId == -1)
            {
                return BadRequest("Invalid CompanyId provided");
            }
                return newRestaurantId;
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        [Authorize(Policy = "RestaurantAdministrator")]
        public ActionResult DeleteRestaurant(int id)
        {
            if (User.HasClaim("RestaurantId", id.ToString()) == false)
            {
                return Unauthorized();
            }
            if (RestaurantsIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Restaurants'  is null.");
            }
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }
            _restaurantsService.DeleteRestaurantAndRelatedEntitiesById(id);
            return Ok();
        }

        // Restaurant details and menu with all states
        // GET: api/Restaurants/Detailed/5
        [HttpGet("Detailed/{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        [Authorize(Policy = "RestaurantAdministrator")]
        public ActionResult<RestaurantDetailedResponse> GetDetailedRestaurant(int id)
        {
            if (User.HasClaim("RestaurantId", id.ToString()) == false)
            {
                return Unauthorized();
            }
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }
            return _restaurantsService.GetRestaurantDetailedResponse(id);
        }

        // Restaurant details and menu with only active state
        // GET: api/Restaurants/Menu/5
        [HttpGet("Menu/{id}")]
        public ActionResult<RestaurantDetailedResponse> GetRestaurantMenu(int id)
        {
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }
            return _restaurantsService.GetRestaurantMenu(id);
        }

        
        // GET: api/Restaurants/RestaurantUsers/5
        [HttpGet("RestaurantUsers/{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        [Authorize(Policy = "RestaurantAdministrator")]
        public ActionResult<List<RestaurantUserResponse>> GetRestaurantUsers(int id)
        {
            if (User.HasClaim("RestaurantId", id.ToString()) == false)
            {
                return Unauthorized();
            }
            if (!RestaurantExists(id))
            {
                return NotFound("Restaurant not found with this id: " + id);
            }
            return _restaurantsService.GetRestaurantUsers(id);
        }
    }
}
