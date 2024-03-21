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
        private readonly UsersService _usersService;
        private readonly RestaurantsService _restaurantsService;

        public RestaurantUsersService(ApplicationContext context, UsersService usersService, RestaurantsService restaurantsService)
        {
            _context = context;
            _usersService = usersService;
            _restaurantsService = restaurantsService;
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
                restaurantUserResponses.Add(GetRestaurantUserResponse(restaurantUser));
            }
            return restaurantUserResponses;
        }

        public RestaurantUserResponse GetRestaurantUserResponse(RestaurantUser restaurantUser)
        {
            RestaurantUserResponse restaurantUserResponse = new()
            {
                RestaurantId = restaurantUser.RestaurantId,
                UserId = restaurantUser.UserId
            };
            return restaurantUserResponse;
        }
    }
}

