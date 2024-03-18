using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantUsersController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RestaurantUsersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/RestaurantUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantUser>>> GetRestaurantUsers()
        {
          if (_context.RestaurantUsers == null)
          {
              return NotFound();
          }
            return await _context.RestaurantUsers.ToListAsync();
        }

        // GET: api/RestaurantUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantUser>> GetRestaurantUser(string id)
        {
          if (_context.RestaurantUsers == null)
          {
              return NotFound();
          }
            var restaurantUser = await _context.RestaurantUsers.FindAsync(id);

            if (restaurantUser == null)
            {
                return NotFound();
            }

            return restaurantUser;
        }

        // PUT: api/RestaurantUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurantUser(string id, RestaurantUser restaurantUser)
        {
            if (id != restaurantUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(restaurantUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RestaurantUsers
        [HttpPost]
        public async Task<ActionResult<RestaurantUser>> PostRestaurantUser(RestaurantUser restaurantUser)
        {
          if (_context.RestaurantUsers == null)
          {
              return Problem("Entity set 'ApplicationContext.RestaurantUsers'  is null.");
          }
            _context.RestaurantUsers.Add(restaurantUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RestaurantUserExists(restaurantUser.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRestaurantUser", new { id = restaurantUser.UserId }, restaurantUser);
        }

        // DELETE: api/RestaurantUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantUser(string id)
        {
            if (_context.RestaurantUsers == null)
            {
                return NotFound();
            }
            var restaurantUser = await _context.RestaurantUsers.FindAsync(id);
            if (restaurantUser == null)
            {
                return NotFound();
            }

            _context.RestaurantUsers.Remove(restaurantUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestaurantUserExists(string id)
        {
            return (_context.RestaurantUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
