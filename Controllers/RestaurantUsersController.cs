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
    public class RestaurantUsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantUsersService _restaurantUsersService;

        public RestaurantUsersController(ApplicationContext context, RestaurantUsersService restaurantUsersService)
        {
            _context = context;
            _restaurantUsersService = restaurantUsersService;
        }

        // GET: api/RestaurantUsers
        [HttpGet]
        public ActionResult<List<RestaurantUserResponse>> GetRestaurantUsers()
        {
            if (_context.RestaurantUsers == null)
            {
                return NotFound();
            }

            return _restaurantUsersService.GetRestaurantUserResponses();
        }

        // GET: api/RestaurantUsers/5
        [HttpGet("{id}")]
        public ActionResult<RestaurantUserResponse> GetRestaurantUser(string id)
        {
            if (_context.RestaurantUsers == null)
            {
                return NotFound();
            }
            
            var restaurantUser = _context.RestaurantUsers.Find(id);

            if (restaurantUser == null)
            {
                return NotFound();
            }

            return _restaurantUsersService.GetRestaurantUserResponse(restaurantUser);
        }

        //    // PUT: api/RestaurantUsers/5
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutRestaurantUser(string id, RestaurantUser restaurantUser)
        //    {
        //        if (id != restaurantUser.UserId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(restaurantUser).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RestaurantUserExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        // POST: api/RestaurantUsers
        [HttpPost]
        public ActionResult<RestaurantUserResponse> PostRestaurantUser(RestaurantUserCreate restaurantUserCreate)
        {
            if (_context.RestaurantUsers == null)
            {
                return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
            }
            
            return _restaurantUsersService.AddRestaurantUser(restaurantUserCreate);

        }

        //    // DELETE: api/RestaurantUsers/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteRestaurantUser(string id)
        //    {
        //        if (_context.RestaurantUsers == null)
        //        {
        //            return NotFound();
        //        }
        //        var restaurantUser = await _context.RestaurantUsers.FindAsync(id);
        //        if (restaurantUser == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.RestaurantUsers.Remove(restaurantUser);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //    private bool RestaurantUserExists(string id)
        //    {
        //        return (_context.RestaurantUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        //    }
    }
}
