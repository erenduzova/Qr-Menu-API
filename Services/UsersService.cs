using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Services
{
	public class UsersService
    {
        private readonly ApplicationContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RolesService _rolesService;
        private readonly UserConverter _userConverter;
        private readonly RestaurantUsersService _restaurantUsersService;

        public UsersService(ApplicationContext context, SignInManager<ApplicationUser> signInManager, RolesService rolesService, UserConverter userConverter, RestaurantUsersService restaurantUsersService)
        {
            _context = context;
            _signInManager = signInManager;
            _rolesService = rolesService;
            _userConverter = userConverter;
            _restaurantUsersService = restaurantUsersService;
        }
        private ApplicationUser GetApplicationUserById(string userId)
        {
            return _context.Users!
                .Include(u => u.State)
                .First(u => u.Id == userId);
        }
        private ApplicationUser GetApplicationUserByUserName(string userName)
        {
            return _context.Users!
                .Include(u => u.State)
                .First(u => u.UserName == userName);
        }

        private List<ApplicationUser> GetApplicationUsers()
        {
            return _context.Users!
                .Include(u => u.State)
                .ToList();
        }

        public ApplicationUserResponse GetApplicationUserResponse(string id)
        {
            ApplicationUser applicationUser = GetApplicationUserById(id);
            return _userConverter.Convert(applicationUser);
        }

        public List<ApplicationUserResponse> GetApplicationUserResponses()
        {
            List<ApplicationUser> applicationUsers = GetApplicationUsers();
            return _userConverter.Convert(applicationUsers);
        }

        public string CreateApplicationUser(ApplicationUserCreate applicationUserCreate, string password)
        {
            ApplicationUser newApplicationUser = _userConverter.Convert(applicationUserCreate);
            _signInManager.UserManager.CreateAsync(newApplicationUser, password).Wait();
            return newApplicationUser.Id;
        }

        public ApplicationUserResponse UpdateApplicationUser(string id, ApplicationUserCreate updatedApplicationUser)
        {
            ApplicationUser existingApplicationUser = GetApplicationUserById(id);
            existingApplicationUser = _userConverter.Convert(existingApplicationUser, updatedApplicationUser);
            _signInManager.UserManager.UpdateAsync(existingApplicationUser).Wait();
            return _userConverter.Convert(existingApplicationUser);
        }

        public void DeleteApplicationUserAndRelatedEntities(ApplicationUser applicationUser)
        {
            applicationUser.StateId = 0;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            if (applicationUser.RestaurantUsers != null)
            {
                _restaurantUsersService.DeleteRestaurantUsers(applicationUser.RestaurantUsers);
            }
            IList<string> roles = _signInManager.UserManager.GetRolesAsync(applicationUser).Result;
            _signInManager.UserManager.RemoveFromRolesAsync(applicationUser, roles).Wait();
        }

        public void DeleteApplicationUserAndRelatedEntitiesById(string id)
        {
            ApplicationUser applicationUser = GetApplicationUserById(id);
            DeleteApplicationUserAndRelatedEntities(applicationUser);
        }

        public Microsoft.AspNetCore.Identity.SignInResult LogIn(string userName, string password)
        {
            ApplicationUser applicationUser = GetApplicationUserByUserName(userName);
            if (applicationUser.StateId != 1)
            {
                return Microsoft.AspNetCore.Identity.SignInResult.NotAllowed;
            }
            return _signInManager.PasswordSignInAsync(applicationUser, password, false, false).Result; 
        }

        public string ResetPasswordGenerateToken(string userName)
        {
            ApplicationUser applicationUser = GetApplicationUserByUserName(userName);
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
        }

        public IdentityResult ResetPasswordValidateToken(string userName, string token, string newPassword)
        {
            ApplicationUser applicationUser = GetApplicationUserByUserName(userName);
            return _signInManager.UserManager.ResetPasswordAsync(applicationUser, token, newPassword).Result; 
        }

        public bool AssignRole(string userId, string roleId)
        {
            ApplicationUser applicationUser = GetApplicationUserById(userId);
            var identityRole = _rolesService.GetRoleById(roleId);
            if (identityRole == null)
            {
                return false;
            }
            _signInManager.UserManager.AddToRoleAsync(applicationUser, identityRole.Name!).Wait();
            return true;
        }

        public bool UnassignRole(string userId, string roleId)
        {
            ApplicationUser applicationUser = GetApplicationUserById(userId);
            var identityRole = _rolesService.GetRoleById(roleId);
            if (identityRole == null)
            {
                return false;
            }
            if (_signInManager.UserManager.IsInRoleAsync(applicationUser, identityRole.Name!).Result)
            {
                return false;
            }
            _signInManager.UserManager.RemoveFromRoleAsync(applicationUser, identityRole.Name!).Wait();
            return true;
        }
    }
}

