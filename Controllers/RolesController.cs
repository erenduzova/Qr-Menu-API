using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qr_Menu_API.Data;
using Qr_Menu_API.Services;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RolesService _rolesService;

        public RolesController(ApplicationContext context, RoleManager<IdentityRole> roleManager, RolesService rolesService)
        {
            _context = context;
            _roleManager = roleManager;
            _rolesService = rolesService;
        }

        // POST: api/Roles
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult PostApplicationRole(string name)
        {
            IdentityRole applicationRole = new IdentityRole(name);
            _rolesService.CreateRole(applicationRole);
            return Ok();
        }

        [HttpDelete("{roleId}")]
        [Authorize(Roles= "Administrator")]
        public ActionResult DeleteApplicationRole(string roleId)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'ApplicationContext.Roles'  is null.");
            }
            var identityRole = _roleManager.FindByIdAsync(roleId).Result;
            if (identityRole == null)
            {
                return NotFound();
            }
            if (User.IsInRole(identityRole.Name!))
            {
                return Problem("This role can not be deleted.");
            }
            _rolesService.DeleteRole(identityRole);
            return Ok();
        }
    }
}

