using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.DetailedResponseDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.Services
{
    public class RestaurantsService
    {
        private readonly ApplicationContext _context;
        private readonly CategoriesService _categoriesService;

        public RestaurantsService(ApplicationContext context, CategoriesService categoriesService)
        {
            _context = context;
            _categoriesService = categoriesService;
        }
        public RestaurantResponse GetRestaurantResponse(Restaurant restaurant)
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
                StateResponse = new StateResponse
                {
                    Id = restaurant.StateId,
                    Name = _context.States!.Find(restaurant.StateId)!.Name
                },
                CompanyId = restaurant.CompanyId,
                CategoryIds = restaurant.Categories?.Select(c => c.Id).ToList()
            };
            return restaurantResponse;
        }

        public List<RestaurantResponse> GetRestaurantsResponses()
        {
            List<Restaurant> restaurants = _context.Restaurants.Include(r => r.Categories).ToList();
            List<RestaurantResponse> restaurantResponses = new();
            restaurants.ForEach(restaurant =>
            {
                restaurantResponses.Add(GetRestaurantResponse(restaurant));
            });
            return restaurantResponses;
        }

        public int CreateRestaurant(RestaurantCreate restaurantCreate)
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

            _context.Restaurants.Add(newRestaurant);
            var company = _context.Companies.Include(c => c.Restaurants).FirstOrDefault(c => c.Id == restaurantCreate.CompanyId);
            if (company != null)
            {
                company.Restaurants.Add(newRestaurant);
            }
            _context.SaveChanges();
            return newRestaurant.Id;
        }

        public RestaurantResponse UpdateRestaurant(Restaurant existingRestaurant, RestaurantCreate updatedRestaurant)
        {
            existingRestaurant.Name = updatedRestaurant.Name;
            existingRestaurant.WebAddress = updatedRestaurant.WebAddress;
            existingRestaurant.Phone = updatedRestaurant.Phone;
            existingRestaurant.PostalCode = updatedRestaurant.PostalCode;
            existingRestaurant.AddressDetails = updatedRestaurant.AddressDetails;

            _context.Update(existingRestaurant);
            _context.SaveChanges();
            return GetRestaurantResponse(existingRestaurant);
        }

        public void DeleteRestaurantAndRelatedEntities(Restaurant restaurant)
        {
            restaurant.StateId = 0;
            _context.Restaurants.Update(restaurant);

            var categories = restaurant.Categories;
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    _categoriesService.DeleteCategoryAndRelatedEntities(category);
                }
            }
            _context.SaveChanges();
        }

        public RestaurantDetailedResponse GetRestaurantDetailedResponse(Restaurant restaurant)
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
                StateResponse = new StateResponse
                {
                    Id = restaurant.StateId,
                    Name = _context.States!.Find(restaurant.StateId)!.Name
                },
                CompanyId = restaurant.CompanyId,
                CategoriesDetailed = new List<CategoryDetailedResponse>()
            };
            if (restaurant.Categories != null)
            {
                foreach (Category category in restaurant.Categories)
                {
                    CategoryDetailedResponse categoryDetailedResponse = _categoriesService.GetDetailedCategoryResponse(category);
                    restaurantDetailedResponse.CategoriesDetailed.Add(categoryDetailedResponse);
                }
            }
            
            return restaurantDetailedResponse;
        }


    }
}
