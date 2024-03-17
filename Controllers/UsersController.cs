using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<List<ApplicationUser>> GetUsers()
        {
            if (_signInManager.UserManager.Users == null)
            {
                return NotFound();
            }
            return _signInManager.UserManager.Users.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<ApplicationUser> GetApplicationUser(string id)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByIdAsync(id).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            return applicationUser;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionResult PutApplicationUser(ApplicationUser applicationUser)
        {
            ApplicationUser? existingApplicationUser = _signInManager.UserManager.FindByIdAsync(applicationUser.Id).Result;
            if (applicationUser == null)
            {
                return NotFound();
            }
            existingApplicationUser.UserName = applicationUser.UserName;
            existingApplicationUser.Email = applicationUser.Email;
            existingApplicationUser.Name = applicationUser.Name;
            existingApplicationUser.PhoneNumber = applicationUser.PhoneNumber;

            _signInManager.UserManager.UpdateAsync(existingApplicationUser).Wait();

            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        public string PostApplicationUser(ApplicationUser applicationUser, string password)
        {
            _signInManager.UserManager.CreateAsync(applicationUser, password).Wait();
            return applicationUser.Id;
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
            applicationUser.StateId = 0;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
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
            Microsoft.AspNetCore.Identity.SignInResult signInResult = _signInManager.PasswordSignInAsync(applicationUser, password, false, false).Result;
            if (signInResult.Succeeded)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }
        }

        // api/Users/ResetPassword
        [HttpPost("ResetPassword")]
        public void ResetPassword(string userName, string password)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return;
            }
            _signInManager.UserManager.RemovePasswordAsync(applicationUser).Wait();
            _signInManager.UserManager.AddPasswordAsync(applicationUser, password).Wait();
            return;
        }

        // api/Users/ResetPasswordGenerateToken
        [HttpPost("ResetPasswordGenerateToken")]
        public string? ResetPasswordGenerateToken(string userName)
        {
            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return null;
            }
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
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
            IdentityResult identityResult = _signInManager.UserManager.ResetPasswordAsync(applicationUser, token, newPassword).Result;
            if (identityResult.Succeeded == false)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok();
        }

    }
}
