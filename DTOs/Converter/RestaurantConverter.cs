using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.DetailedResponseDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.DTOs.Converter
{
	public class RestaurantConverter
	{
        private readonly StateConverter _stateConverter;
        private readonly CategoryConverter _categoryConverter;

        public RestaurantConverter(StateConverter stateConverter, CategoryConverter categoryConverter)
        {
            _stateConverter = stateConverter;
            _categoryConverter = categoryConverter;
        }

        public Restaurant Convert(RestaurantCreate restaurantCreate)
		{
            Restaurant newRestaurant = new()
            {
                Name = restaurantCreate.Name,
                CompanyId = restaurantCreate.CompanyId,
                WebAddress = restaurantCreate.WebAddress,
                Phone = restaurantCreate.Phone,
                PostalCode = restaurantCreate.PostalCode,
                AddressDetails = restaurantCreate.AddressDetails,
                RegisterDate = DateTime.Now,
                StateId = (byte)1,
                Categories = new List<Category>()
            };
            return newRestaurant;
		}

        public RestaurantResponse Convert(Restaurant restaurant)
        {
            RestaurantResponse restaurantResponse = new()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                WebAddress = restaurant.WebAddress,
                Phone = restaurant.Phone,
                PostalCode = restaurant.PostalCode,
                AddressDetails = restaurant.AddressDetails,
                RegisterDate = restaurant.RegisterDate,
                StateResponse = _stateConverter.Convert(restaurant.State!),
                CompanyId = restaurant.CompanyId,
                CategoryIds = restaurant.Categories?.Select(c => c.Id).ToList()
            };
            return restaurantResponse;
        }

		public List<RestaurantResponse> Convert(List<Restaurant> restaurants)
		{
            List<RestaurantResponse> restaurantResponses = new List<RestaurantResponse>();
            foreach (Restaurant restaurant in restaurants)
            {
                restaurantResponses.Add(Convert(restaurant));
            }
            return restaurantResponses;
        }

        public Restaurant Convert(Restaurant existingRestaurant, RestaurantCreate updatedRestaurant)
		{
            existingRestaurant.Name = updatedRestaurant.Name;
            existingRestaurant.WebAddress = updatedRestaurant.WebAddress;
            existingRestaurant.Phone = updatedRestaurant.Phone;
            existingRestaurant.PostalCode = updatedRestaurant.PostalCode;
            existingRestaurant.AddressDetails = updatedRestaurant.AddressDetails;

            return existingRestaurant;
        }

        public RestaurantDetailedResponse ConvertDetailed(Restaurant restaurant)
        {
            RestaurantDetailedResponse restaurantDetailedResponse = new()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                WebAddress = restaurant.WebAddress,
                Phone = restaurant.Phone,
                PostalCode = restaurant.PostalCode,
                AddressDetails = restaurant.AddressDetails,
                RegisterDate = restaurant.RegisterDate,
                StateResponse = _stateConverter.Convert(restaurant.State!),
                CompanyId = restaurant.CompanyId,
                CategoriesDetailed = new List<CategoryDetailedResponse>()
            };
            if (restaurant.Categories != null)
            {
                foreach (Category category in restaurant.Categories)
                {
                    CategoryDetailedResponse categoryDetailedResponse = _categoryConverter.ConvertDetailed(category);
                    restaurantDetailedResponse.CategoriesDetailed.Add(categoryDetailedResponse);
                }
            }
            return restaurantDetailedResponse;
        }

    }
}

