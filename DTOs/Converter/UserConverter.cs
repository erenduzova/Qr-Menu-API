using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.DTOs.Converter
{
	public class UserConverter
	{
        private readonly StateConverter _stateConverter;

        public UserConverter(StateConverter stateConverter)
        {
            _stateConverter = stateConverter;
        }

        public ApplicationUser Convert(ApplicationUserCreate applicationUserCreate)
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
                RestaurantUsers = new List<RestaurantUser>()
            };
            return newApplicationUser;
        }

        public ApplicationUserResponse Convert(ApplicationUser applicationUser)
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
                StateResponse = _stateConverter.Convert(applicationUser.State!)
            };
            return applicationUserResponse;
        }

        public List<ApplicationUserResponse> Convert(List<ApplicationUser> applicationUsers)
        {
            List<ApplicationUserResponse> applicationUserResponses = new List<ApplicationUserResponse>();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                applicationUserResponses.Add(Convert(applicationUser));
            }
            return applicationUserResponses;
        }

        public ApplicationUser Convert(ApplicationUser existingApplicationUser, ApplicationUserCreate updatedApplicationUser)
        {
            existingApplicationUser!.UserName = updatedApplicationUser.UserName;
            existingApplicationUser.Email = updatedApplicationUser.Email;
            existingApplicationUser.Name = updatedApplicationUser.Name;
            existingApplicationUser.PhoneNumber = updatedApplicationUser.PhoneNumber;
            return existingApplicationUser;
        }
    }
}

