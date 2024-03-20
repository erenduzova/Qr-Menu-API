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
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly CategoriesService _categoriesService;

        public CategoriesController(ApplicationContext context, CategoriesService categoriesService)
        {
            _context = context;
            _categoriesService = categoriesService;
        }

        // GET: api/Categories
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<CategoryResponse>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            return _categoriesService.GetCategoriesResponses();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            var category = _context.Categories.Include(c => c.Foods).FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return _categoriesService.GetCategoryResponse(category);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<CategoryResponse> PutCategory(int id, CategoryCreate updatedCategory)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            var existingCategory = _context.Categories.Include(c => c.Foods).FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return BadRequest();
            }
            return _categoriesService.UpdateCategory(existingCategory, updatedCategory);
        }

        // POST: api/Categories
        [HttpPost]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult<int> PostCategory(CategoryCreate categoryCreate)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            if (_context.States == null)
            {
                return Problem("Entity set 'ApplicationContext.States'  is null.");
            }
            return _categoriesService.CreateCategory(categoryCreate);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantAdministrator")]
        public ActionResult DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            var category = _context.Categories.Include(c => c.Foods).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _categoriesService.DeleteCategoryAndRelatedEntities(category);
            return Ok();
        }

    }
}
