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

        private bool CategoriesIsNull()
        {
            return _context.Categories == null;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories!
                .Any(c => c.Id == id);
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<CategoryResponse>> GetCategories()
        {
            if (CategoriesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            return _categoriesService.GetCategoriesResponses();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            if (CategoriesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            if (!CategoryExists(id))
            {
                return NotFound("Category not found with this id: " + id);
            }
            return _categoriesService.GetCategoryResponse(id);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public ActionResult<CategoryResponse> PutCategory(int id, CategoryCreate updatedCategory)
        {
            if (CategoriesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            if (!CategoryExists(id))
            {
                return NotFound("Category not found with this id: " + id);
            }
            return _categoriesService.UpdateCategory(id, updatedCategory);
        }

        // POST: api/Categories
        [HttpPost]
        public ActionResult<int> PostCategory(CategoryCreate categoryCreate)
        {
            if (CategoriesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            int newCategoryId = _categoriesService.CreateCategory(categoryCreate);
            if (newCategoryId == -1)
            {
                return BadRequest("Invalid restaurantId provided.");
            }
            return Ok(newCategoryId);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            if (CategoriesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Categories'  is null.");
            }
            if (!CategoryExists(id))
            {
                return NotFound("Category not found with this id: " + id);
            }
            _categoriesService.DeleteCategoryAndRelatedEntitiesById(id);
            return Ok();
        }

    }
}
