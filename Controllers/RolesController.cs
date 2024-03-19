using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qr_Menu_API.Services;

namespace Qr_Menu_API.Controllers
{
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RolesService _rolesService;

        public RolesController(RoleManager<IdentityRole> roleManager, RolesService rolesService)
        {
            _roleManager = roleManager;
            _rolesService = rolesService;
        }

        // POST: api/Roles
        [HttpPost]
        public ActionResult PostApplicationRole(string name)
        {
            IdentityRole applicationRole = new IdentityRole(name);
            _rolesService.CreateRole(applicationRole);
            return Ok();
        }
    }
}

