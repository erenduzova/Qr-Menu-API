using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
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
        private readonly RestaurantConverter _restaurantConverter;

        public RestaurantsService(ApplicationContext context, CategoriesService categoriesService, RestaurantConverter restaurantConverter)
        {
            _context = context;
            _categoriesService = categoriesService;
            _restaurantConverter = restaurantConverter;
        }

        private Restaurant GetRestaurant(int restaurantId)
        {
            return _context.Restaurants!
                .Include(r => r.State)
                .First(r => r.Id == restaurantId);
        }

        private List<Restaurant> GetRestaurants()
        {
            return _context.Restaurants!
                .Include(r => r.State)
                .ToList();
        }

        private Restaurant GetRestaurantWithCategories(int restaurantId)
        {
            return _context.Restaurants!
                .Include(r => r.Categories)
                .Include(r => r.State)
                .First(r => r.Id == restaurantId);
        }

        private List<Restaurant> GetRestaurantsWithCategories()
        {
            return _context.Restaurants!
                .Include(c => c.Categories)
                .Include(c => c.State)
                .ToList();
        }

        private Restaurant GetRestaurantWithCategoriesAndUsers(int companyId)
        {
            return _context.Restaurants!
                .Include(c => c.Categories)
                .Include(c => c.State)
                .Include(c => c.Users)
                .First(c => c.Id == companyId);
        }

        private List<Restaurant> GetRestaurantsWithCategoriesAndUsers()
        {
            return _context.Restaurants!
                .Include(c => c.Categories)
                .Include(c => c.State)
                .Include(c => c.Users)
                .ToList();
        }

        public RestaurantResponse GetRestaurantResponse(int id)
        {
            Restaurant foundRestaurant = GetRestaurantWithCategories(id);
            return _restaurantConverter.Convert(foundRestaurant);
        }

        public List<RestaurantResponse> GetRestaurantsResponses()
        {
            List<Restaurant> restaurants = GetRestaurantsWithCategories();
            return _restaurantConverter.Convert(restaurants);
        }

        public int CreateRestaurant(RestaurantCreate restaurantCreate)
        {
            Restaurant newRestaurant = _restaurantConverter.Convert(restaurantCreate);
            _context.Restaurants!.Add(newRestaurant);
            _context.SaveChanges();
            return newRestaurant.Id;
        }

        public RestaurantResponse UpdateRestaurant(int id, RestaurantCreate updatedRestaurant)
        {
            Restaurant existingRestaurant = GetRestaurantWithCategories(id);
            existingRestaurant  = _restaurantConverter.Convert(existingRestaurant, updatedRestaurant);
            _context.Update(existingRestaurant);
            _context.SaveChanges();
            return _restaurantConverter.Convert(existingRestaurant);
        }

        public void DeleteRestaurantAndRelatedEntities(Restaurant restaurant)
        {
            restaurant!.StateId = 0;
            _context.Restaurants!.Update(restaurant);

            var categories = restaurant.Categories;
            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    _categoriesService.DeleteCategoryAndRelatedEntities(category);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteRestaurantAndRelatedEntitiesById(int id)
        {
            Restaurant restaurant = GetRestaurantWithCategoriesAndUsers(id);
            DeleteRestaurantAndRelatedEntities(restaurant);
        }

        public RestaurantDetailedResponse GetRestaurantDetailedResponse(int id)
        {
            Restaurant restaurant = GetRestaurantWithCategories(id);
            return _restaurantConverter.ConvertDetailed(restaurant);
        }


    }
}
