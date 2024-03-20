using System;
using Microsoft.AspNetCore.Identity;

namespace Qr_Menu_API.Services
{
	public class RolesService
	{
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesService(RoleManager<IdentityRole> roleManager)
        {
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
    }
}

