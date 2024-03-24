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
    public class RestaurantUsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantUsersService _restaurantUsersService;

        public RestaurantUsersController(ApplicationContext context, RestaurantUsersService restaurantUsersService)
        {
            _context = context;
            _restaurantUsersService = restaurantUsersService;
        }

        private bool RestaurantUsersIsNull()
        {
            return _context.RestaurantUsers == null;
        }

        private bool RestaurantUserExists(int restaurantId, string userId)
        {
            return _context.RestaurantUsers!
                .Any(ru => ru.RestaurantId == restaurantId && ru.UserId == userId);
        }

        // GET: api/RestaurantUsers
        [HttpGet]
        [Authorize]
        public ActionResult<List<RestaurantUserResponse>> GetRestaurantUsers()
        {
            if (RestaurantUsersIsNull())
            {
                return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
            }
            return _restaurantUsersService.GetRestaurantUserResponses();
        }

        // GET: api/RestaurantUsers/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RestaurantUserResponse> GetRestaurantUser(int restaurantId, string userId)
        {
            if (RestaurantUsersIsNull())
            {
                return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
            }
            if (!RestaurantUserExists(restaurantId,userId))
            {
                return NotFound("RestaurantUser not found with this restaurantId: " + restaurantId + " and this userId: " + userId);
            }
            return _restaurantUsersService.GetRestaurantUserResponse(restaurantId, userId);
        }

        // POST: api/RestaurantUsers
        [HttpPost]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<RestaurantUserResponse> PostRestaurantUser(RestaurantUserCreate restaurantUserCreate)
        {
            if (RestaurantUsersIsNull())
            {
                return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
            }
            return _restaurantUsersService.AddRestaurantUser(restaurantUserCreate);

        }

        // DELETE: api/RestaurantUsers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,CompanyAdministrator,RestaurantAdministrator")]
        public ActionResult DeleteRestaurantUser(int restaurantId, string userId)
        {
            if (RestaurantUsersIsNull())
            {
                return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
            }
            if (!RestaurantUserExists(restaurantId, userId))
            {
                return NotFound("RestaurantUser not found with this restaurantId: " + restaurantId + " and this userId: " + userId);
            }
            _restaurantUsersService.DeleteRestaurantUserByRestaurantIdAndUserId(restaurantId, userId);
            return Ok();
        }

    }
}
