using System;
using Microsoft.AspNetCore.Mvc;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Services
{
	public class RestaurantUsersService
	{
        private readonly ApplicationContext _context;
        private readonly RestaurantUserConverter _restaurantUserConverter;

        public RestaurantUsersService(ApplicationContext context, RestaurantUserConverter restaurantUserConverter)
        {
            _context = context;
            _restaurantUserConverter = restaurantUserConverter;
        }

        private List<RestaurantUser> GetRestaurantUsersByRestaurantId(int restaurantId)
        {
            return _context.RestaurantUsers!.Where(ru => ru.RestaurantId == restaurantId).ToList();
        }
        private List<RestaurantUser> GetRestaurantUsersByUserId(string userId)
        {
            return _context.RestaurantUsers!.Where(ru => ru.UserId == userId).ToList();
        }
        private List<RestaurantUser> GetRestaurantUsers()
        {
            return _context.RestaurantUsers!.ToList();
        }
        private RestaurantUser GetRestaurantUser(int restaurantId, string userId)
        {
            return _context.RestaurantUsers!.First(ru => ru.RestaurantId == restaurantId && ru.UserId == userId);
        }

        public RestaurantUserResponse AddRestaurantUser(RestaurantUserCreate restaurantUserCreate)
        {
            RestaurantUser restaurantUser = _restaurantUserConverter.Convert(restaurantUserCreate);
            _context.RestaurantUsers!.Add(restaurantUser);
            _context.SaveChanges();
            return _restaurantUserConverter.Convert(restaurantUser);
        }

        public List<RestaurantUserResponse> GetRestaurantUserResponses()
        {
            List<RestaurantUser> restaurantUsers = GetRestaurantUsers();
            return _restaurantUserConverter.Convert(restaurantUsers);
        }

        public RestaurantUserResponse GetRestaurantUserResponse(int restaurantId, string userId)
        {
            RestaurantUser restaurantUser = GetRestaurantUser(restaurantId, userId);
            return _restaurantUserConverter.Convert(restaurantUser);
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

