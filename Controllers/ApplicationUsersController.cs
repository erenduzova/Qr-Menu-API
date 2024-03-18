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
    public class ApplicationUsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationUsersService _applicationUsersService;

        public ApplicationUsersController(SignInManager<ApplicationUser> signInManager, ApplicationUsersService applicationUsersService)
        {
            _signInManager = signInManager;
            _applicationUsersService = applicationUsersService;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<List<ApplicationUserResponse>> GetUsers()
        {
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            return _applicationUsersService.GetApplicationUserResponses();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<ApplicationUserResponse> GetApplicationUser(string id)
        {
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            return _applicationUsersService.GetApplicationUserResponse(applicationUser);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionResult<ApplicationUserResponse> PutApplicationUser(string id, ApplicationUserCreate updatedApplicationUser)
        {
            ApplicationUser? existingApplicationUser = _signInManager.UserManager.FindByIdAsync(id).Result;
            if (existingApplicationUser == null)
            {
                return NotFound();
            }
            
            return _applicationUsersService.UpdateApplicationUser(existingApplicationUser, updatedApplicationUser);
        }

        // POST: api/Users
        [HttpPost]
        public string PostApplicationUser(ApplicationUserCreate applicationUserCreate, string password)
        {
            return _applicationUsersService.CreateApplicationUser(applicationUserCreate, password);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public ActionResult DeleteApplicationUser(string id)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            _applicationUsersService.DeleteApplicationUserAndRelatedEntities(applicationUser);
            return Ok();
        }

        // api/Users/LogIn
        [HttpPost("LogIn")]
        public ActionResult LogIn(string userName, string password)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = _applicationUsersService.LogIn(applicationUser, password);
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
            return _applicationUsersService.ResetPasswordGenerateToken(applicationUser);
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
            IdentityResult identityResult = _applicationUsersService.ResetPasswordValidateToken(applicationUser, token, newPassword);
            if (identityResult.Succeeded == false)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok();
        }

    }
}
