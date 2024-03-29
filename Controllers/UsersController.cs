﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;
using System.Security.Claims;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UsersService _usersService;

        public UsersController(SignInManager<ApplicationUser> signInManager, UsersService usersService)
        {
            _signInManager = signInManager;
            _usersService = usersService;
        }

        private bool UsersIsNull()
        {
            return _signInManager.UserManager.Users == null;
        }

        private bool UserExistsById(string id)
        {
            return _signInManager.UserManager.Users.Any(u => u.Id == id);
        }

        private bool UserExistsByUserName(string userName)
        {
            return _signInManager.UserManager.Users.Any(u => u.UserName == userName);
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
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
        [Authorize]
        public ActionResult<ApplicationUserResponse> GetApplicationUser(string id)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExistsById(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            return _usersService.GetApplicationUserResponse(id);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<ApplicationUserResponse> PutApplicationUser(string id, ApplicationUserCreate updatedApplicationUser)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Unauthorized();
            }
            if (!UserExistsById(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            return _usersService.UpdateApplicationUser(id, updatedApplicationUser);
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<string> PostApplicationUser(ApplicationUserCreate applicationUserCreate, string password)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            string newUserId = _usersService.CreateApplicationUser(applicationUserCreate, password);
            if (newUserId == "-1")
            {
                return BadRequest("Invalid CompanyId provided");
            }
            return Ok(newUserId);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteApplicationUser(string id)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExistsById(id))
            {
                return NotFound("User not found with this id: " + id);
            }
            _usersService.DeleteApplicationUserAndRelatedEntitiesById(id);
            return Ok();
        }

        // api/Users/LogIn
        [HttpPost("LogIn")]
        public ActionResult LogIn(string userName, string password)
        {
            if (!UserExistsByUserName(userName))
            {
                return NotFound("User not found with this user name: " + userName);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = _usersService.LogIn(userName, password);

            if (signInResult.Succeeded)
            {
                return Ok("LogIn succesfull");
            }
            else if (signInResult.IsLockedOut)
            {
                return BadRequest("Your account is locked out. Please try again later.");
            }
            else if (signInResult.IsNotAllowed)
            {
                return BadRequest("Login is not allowed for this user.");
            }
            else if (signInResult.RequiresTwoFactor)
            {
                return BadRequest("Two-factor authentication is required for this user.");
            }
            else
            {
                return BadRequest("An unknown error occurred during login.");
            }
        }

        // api/Users/ResetPasswordGenerateToken
        [HttpPost("ResetPasswordGenerateToken")]
        public ActionResult<string>? ResetPasswordGenerateToken(string userName)
        {
            if (!UserExistsByUserName(userName))
            {
                return NotFound("User not found with this user name: " + userName);
            }
            return _usersService.ResetPasswordGenerateToken(userName);
        }

        // api/Users/ResetPasswordValidateToken
        [HttpPost("ResetPasswordValidateToken")]
        public ActionResult<String> ResetPasswordValidateToken(string userName, string token, string newPassword)
        {
            if (!UserExistsByUserName(userName))
            {
                return NotFound("User not found with this user name: " + userName);
            }
            IdentityResult identityResult = _usersService.ResetPasswordValidateToken(userName, token, newPassword);
            if (identityResult.Succeeded == false)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok();
        }

        // api/Users/AssignRole
        [HttpPost("AssignRole")]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(string userId, string roleId)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExistsById(userId))
            {
                return NotFound("User not found with this id: " + userId);
            }
            if (_usersService.AssignRole(userId, roleId))
            {
                return Ok();
            }
            return NotFound("Role not found with this id: " + roleId);
        }

        // api/Users/UnassignRole
        [HttpPut("UnassignRole")]
        [Authorize(Roles = "Administrator")]
        public ActionResult UnassignRole(string userId, string roleId)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExistsById(userId))
            {
                return NotFound("User not found with this id: " + userId);
            }
            if (_usersService.UnassignRole(userId, roleId))
            {
                return Ok("Role Unassigned");
            }
            return Problem("Error occured while removing role");
        }

        // api/Users/GiveRestaurantClaim/54d5/4
        [HttpPost("GiveRestaurantClaim/{userId}/{restaurantId}")]
        [Authorize(Roles = "CompanyAdministrator")]
        public ActionResult GiveRestaurantAdministratorClaim(string userId, int restaurantId)
        {
            if (UsersIsNull())
            {
                return Problem("Entity set '_signInManager.UserManager.Users'  is null.");
            }
            if (!UserExistsById(userId))
            {
                return NotFound("User not found with this id: " + userId);
            }
            _usersService.GiveRestaurantAdministratorClaim(userId, restaurantId);
            return Ok();
        }
    }
}
