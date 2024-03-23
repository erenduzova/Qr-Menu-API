using System;
using Microsoft.AspNetCore.Identity;
using Qr_Menu_API.Data;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Services
{
	public class RolesService
	{
        private readonly ApplicationContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesService(ApplicationContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public void CreateRole(IdentityRole applicationRole)
        {
            _roleManager.CreateAsync(applicationRole).Wait();
        }

        public void DeleteRole(IdentityRole identityRole)
        {
            _roleManager.DeleteAsync(identityRole).Wait();
        }

        public IdentityRole? GetRoleById(string roleId)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == roleId);
        }
    }
}

