using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
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

        public UsersService(ApplicationContext context, SignInManager<ApplicationUser> signInManager, RolesService rolesService)
        {
            _context = context;
            _signInManager = signInManager;
            _rolesService = rolesService;
        }

        public ApplicationUserResponse GetApplicationUserResponse(ApplicationUser applicationUser)
        {
            ApplicationUserResponse applicationUserResponse = new()
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Name = applicationUser.Name,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                RegisterDate = applicationUser.RegisterDate,
                CompanyId = applicationUser.CompanyId,
                StateResponse = new StateResponse
                {
                    Id = applicationUser.StateId,
                    Name = _context.States!.Find(applicationUser.StateId)!.Name
                }
            };
            return applicationUserResponse;
        }

        public List<ApplicationUserResponse> GetApplicationUserResponses()
        {
            List<ApplicationUser> applicationUsers = _context.Users.ToList();
            List<ApplicationUserResponse> applicationUserResponses = new();
            applicationUsers.ForEach(applicationUser =>
            {
                applicationUserResponses.Add(GetApplicationUserResponse(applicationUser));
            });
            return applicationUserResponses;
        }

        public string CreateApplicationUser(ApplicationUserCreate applicationUserCreate, string password)
        {
            ApplicationUser newApplicationUser = new()
            {
                UserName = applicationUserCreate.UserName,
                Name = applicationUserCreate.Name,
                Email = applicationUserCreate.Email,
                PhoneNumber = applicationUserCreate.PhoneNumber,
                RegisterDate = DateTime.Now,
                CompanyId = applicationUserCreate.CompanyId,
                StateId = (byte)1,
            };
            _signInManager.UserManager.CreateAsync(newApplicationUser, password).Wait();
            return newApplicationUser.Id;
        }

        public ApplicationUserResponse UpdateApplicationUser(ApplicationUser existingApplicationUser, ApplicationUserCreate updatedApplicationUser)
        {
            existingApplicationUser!.UserName = updatedApplicationUser.UserName;
            existingApplicationUser.Email = updatedApplicationUser.Email;
            existingApplicationUser.Name = updatedApplicationUser.Name;
            existingApplicationUser.PhoneNumber = updatedApplicationUser.PhoneNumber;

            _signInManager.UserManager.UpdateAsync(existingApplicationUser).Wait();
            return GetApplicationUserResponse(existingApplicationUser);
        }

        public void DeleteApplicationUserAndRelatedEntities(ApplicationUser applicationUser)
        {
            applicationUser.StateId = 0;
            _signInManager.UserManager.UpdateAsync(applicationUser).Wait();
            
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
    }
}

