using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UsersService _usersService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(SignInManager<ApplicationUser> signInManager, UsersService usersService, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _usersService = usersService;
            _roleManager = roleManager;
        }

        private bool UsersIsNull()
        {
            return _signInManager.UserManager.Users == null;
        }

        private bool UserExists(string id)
        {
            return _signInManager.UserManager.Users.Any(u => u.Id == id);
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<List<ApplicationUserResponse>> GetUsers()
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            return _usersService.GetApplicationUserResponses();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<ApplicationUserResponse> GetApplicationUser(string id)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExists(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            return _usersService.GetApplicationUserResponse(id);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionResult<ApplicationUserResponse> PutApplicationUser(string id, ApplicationUserCreate updatedApplicationUser)
        {
            if (!UserExists(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            return _usersService.UpdateApplicationUser(id, updatedApplicationUser);
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult<string> PostApplicationUser(ApplicationUserCreate applicationUserCreate, string password)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            return _usersService.CreateApplicationUser(applicationUserCreate, password);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public ActionResult DeleteApplicationUser(string id)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExists(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            _usersService.DeleteApplicationUserAndRelatedEntitiesById(id);
            return Ok();
        }

        // api/Users/LogIn
        [HttpPost("LogIn")]
        public ActionResult LogIn(string userName, string password)
        {ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = _usersService.LogIn(applicationUser, password);
            if (signInResult.Succeeded)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }
        }

        //// api/Users/ResetPassword
        //[HttpPost("ResetPassword")]
        //public void ResetPassword(string userName, string password)
        //{
        //    ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
        //    if (applicationUser == null)
        //    {
        //        return;
        //    }
        //    _signInManager.UserManager.RemovePasswordAsync(applicationUser).Wait();
        //    _signInManager.UserManager.AddPasswordAsync(applicationUser, password).Wait();
        //    return;
        //}

        // api/Users/ResetPasswordGenerateToken
        [HttpPost("ResetPasswordGenerateToken")]
        public string? ResetPasswordGenerateToken(string userName)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return null;
            }
            return _usersService.ResetPasswordGenerateToken(applicationUser);
        }

        // api/Users/ResetPasswordValidateToken
        [HttpPost("ResetPasswordValidateToken")]
        public ActionResult<String> ResetPasswordValidateToken(string userName, string token, string newPassword)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            IdentityResult identityResult = _usersService.ResetPasswordValidateToken(applicationUser, token, newPassword);
            if (identityResult.Succeeded == false)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok();
        }

        // api/Users/AssignRole
        [HttpPost("AssignRole")]
        public ActionResult AssignRole(string userId, string roleId)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(userId).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            IdentityRole? identityRole = _roleManager.FindByIdAsync(roleId).Result;
            if (identityRole == null)
            {
                return NotFound();
            }
            _usersService.AssignRole(applicationUser, identityRole);
            return Ok();
                
        }

        // api/Users/UnassignRole
        [HttpPut("UnassignRole")]
        public ActionResult UnassignRole(string userId, string roleId)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(userId).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            IdentityRole? identityRole = _roleManager.FindByIdAsync(roleId).Result;
            if (identityRole == null)
            {
                return NotFound();
            }
            if (_signInManager.UserManager.IsInRoleAsync(applicationUser, identityRole.Name!).Result)
            {
                return NotFound();
            }
            _usersService.UnassignRole(applicationUser, identityRole);
            return Ok();
        }
    }
}
