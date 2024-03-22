using System;
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

        public UsersService(ApplicationContext context, SignInManager<ApplicationUser> signInManager, RolesService rolesService, UserConverter userConverter)
        {
            _context = context;
            _signInManager = signInManager;
            _rolesService = rolesService;
            _userConverter = userConverter;
        }
        private ApplicationUser GetApplicationUser(string userId)
        {
            return _context.Users!
                .Include(u => u.State)
                .First(u => u.Id == userId);
        }

        private List<ApplicationUser> GetApplicationUsers()
        {
            return _context.Users!
                .Include(u => u.State)
                .ToList();
        }

        public ApplicationUserResponse GetApplicationUserResponse(string id)
        {
            ApplicationUser applicationUser = GetApplicationUser(id);
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
            ApplicationUser existingApplicationUser = GetApplicationUser(id);
            existingApplicationUser = _userConverter.Convert(existingApplicationUser, updatedApplicationUser);
            _signInManager.UserManager.UpdateAsync(existingApplicationUser).Wait();
            return _userConverter.Convert(existingApplicationUser);
        }

        public void DeleteApplicationUserAndRelatedEntities(ApplicationUser applicationUser)
        {
            applicationUser.StateId = 0;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            _signInManager.SignOutAsync().Wait();
        }

        public void DeleteApplicationUserAndRelatedEntitiesById(string id)
        {
            ApplicationUser applicationUser = _context.Users.First(u => u.Id == id);
            DeleteApplicationUserAndRelatedEntities(applicationUser);
        }

        public Microsoft.AspNetCore.Identity.SignInResult LogIn(ApplicationUser applicationUser, string password)
        {
         return _signInManager.PasswordSignInAsync(applicationUser, password, false, false).Result; 
        }

        public string ResetPasswordGenerateToken(ApplicationUser applicationUser)
        {
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
        }

        public IdentityResult ResetPasswordValidateToken(ApplicationUser applicationUser, string token, string newPassword)
        {
            return _signInManager.UserManager.ResetPasswordAsync(applicationUser, token, newPassword).Result; 
        }

        public void AssignRole(ApplicationUser applicationUser,IdentityRole identityRole)
        {
            _signInManager.UserManager.AddToRoleAsync(applicationUser, identityRole.Name!).Wait();
        }

        public void UnassignRole(ApplicationUser applicationUser, IdentityRole identityRole)
        {
            _signInManager.UserManager.RemoveFromRoleAsync(applicationUser, identityRole.Name!).Wait();
        }
    }
}

