using System;
using Microsoft.AspNetCore.Mvc;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Services
{
	public class RestaurantUsersService
	{
        private readonly ApplicationContext _context;

        public RestaurantUsersService(ApplicationContext context)
        {
            _context = context;
        }

        private List<RestaurantUser> GetRestaurantUsersByRestaurantId(int restaurantId)
        {
            return _context.RestaurantUsers!.Where(ru => ru.RestaurantId == restaurantId).ToList();
        }
        private List<RestaurantUser> GetRestaurantUsersByUserId(string userId)
        {
            return _context.RestaurantUsers!.Where(ru => ru.UserId == userId).ToList();
        }
        private RestaurantUser GetRestaurantUser(int restaurantId, string userId)
        {
            return _context.RestaurantUsers!.First(ru => ru.RestaurantId == restaurantId && ru.UserId == userId);
        }

        public RestaurantUserResponse AddRestaurantUser(RestaurantUserCreate restaurantUserCreate)
        {
            RestaurantUser restaurantUser = new()
            {
                RestaurantId = restaurantUserCreate.RestaurantId,
                UserId = restaurantUserCreate.UserId
            };
            _context.RestaurantUsers!.Add(restaurantUser);
            RestaurantUserResponse restaurantUserResponse = new()
            {
                RestaurantId = restaurantUser.RestaurantId,
                UserId = restaurantUser.UserId
            };
            return restaurantUserResponse;
        }

        public List<RestaurantUserResponse> GetRestaurantUserResponses()
        {
            var restaurantUsers = _context.RestaurantUsers!.ToList();
            List<RestaurantUserResponse> restaurantUserResponses = new List<RestaurantUserResponse>();
            foreach (RestaurantUser restaurantUser in restaurantUsers)
            {
                RestaurantUserResponse restaurantUserResponse = new()
                {
                    RestaurantId = restaurantUser.RestaurantId,
                    UserId = restaurantUser.UserId
                };
                restaurantUserResponses.Add(restaurantUserResponse);
            }
            return restaurantUserResponses;
        }

        public RestaurantUserResponse GetRestaurantUserResponse(int restaurantId, string userId)
        {
            RestaurantUser restaurantUser = GetRestaurantUser(restaurantId, userId);
            RestaurantUserResponse restaurantUserResponse = new()
            {
                RestaurantId = restaurantUser.RestaurantId,
                UserId = restaurantUser.UserId
            };
            return restaurantUserResponse;
        }

        public void DeleteRestaurantUsersByRestaurantId(int restaurantId)
        {
            List<RestaurantUser> restaurantUsers = GetRestaurantUsersByRestaurantId(restaurantId);
            DeleteRestaurantUsers(restaurantUsers);
        }

        public void DeleteRestaurantUsersByUserId(string userId)
        {
            List<RestaurantUser> restaurantUsers = GetRestaurantUsersByUserId(userId);
            DeleteRestaurantUsers(restaurantUsers);
        }

        public void DeleteRestaurantUserByRestaurantIdAndUserId(int restaurantId, string userId)
        {
            RestaurantUser restaurantUser = GetRestaurantUser(restaurantId, userId);
            DeleteRestaurantUser(restaurantUser);
        }

        public void DeleteRestaurantUsers(ICollection<RestaurantUser> restaurantUsers)
        {
            foreach (RestaurantUser restaurantUser in restaurantUsers)
            {
                DeleteRestaurantUser(restaurantUser);
            }
        }

        public void DeleteRestaurantUser(RestaurantUser restaurantUser)
        {
            _context.RestaurantUsers!.Remove(restaurantUser);
            _context.SaveChanges();
        }
    }
}

