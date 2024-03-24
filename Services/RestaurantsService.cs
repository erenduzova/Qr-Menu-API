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
        private readonly RestaurantUsersService _restaurantUsersService;

        public RestaurantsService(ApplicationContext context, CategoriesService categoriesService, RestaurantConverter restaurantConverter, RestaurantUsersService restaurantUsersService)
        {
            _context = context;
            _categoriesService = categoriesService;
            _restaurantConverter = restaurantConverter;
            _restaurantUsersService = restaurantUsersService;
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

        private Restaurant GetRestaurantWithRelatedEntitiesWithStates(int restaurantId)
        {
            return _context.Restaurants!
                 .Include(r => r.State)
                 .Include(r => r.Categories)
                     .ThenInclude(c => c.State)
                 .Include(r => r.Categories)
                     .ThenInclude(c => c.Foods)
                         .ThenInclude(f => f.State)
                 .First(r => r.Id == restaurantId);
        }

        private Restaurant GetRestaurantAndRelatedEntitesWithActiveState(int restaurantId)
        {
            var restaurant = _context.Restaurants!
                .Include(r => r.State)
                .Include(r => r.Categories)
                .ThenInclude(c => c.Foods)
                .First(r => r.Id == restaurantId);

            restaurant.Categories = restaurant.Categories?
                .Where(c => c.StateId == 1)
                .ToList();

            foreach (var category in restaurant.Categories)
            {
                category.Foods = category.Foods?
                    .Where(f => f.StateId == 1)
                    .ToList();
            }
            return restaurant;
        }

        private List<Restaurant> GetRestaurantsWithCategories()
        {
            return _context.Restaurants!
                .Include(c => c.Categories)
                .Include(c => c.State)
                .ToList();
        }
        private Restaurant GetRestaurantWithCategoriesAndRestaurantUsers(int restaurantId)
        {
            return _context.Restaurants!
                .Include(r => r.Categories)
                .Include(r => r.State)
                .Include(r => r.RestaurantUsers)
                .First(r => r.Id == restaurantId);
        }
        private Restaurant GetRestaurantWithRestaurantUsers(int restaurantId)
        {
            return _context.Restaurants!
                .Include(r => r.RestaurantUsers)
                .First(r => r.Id == restaurantId);
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies!
                .Any(r => r.Id == id && r.StateId != 0);
        }
        public RestaurantResponse GetRestaurantResponse(int id)
        {
            Restaurant foundRestaurant = GetRestaurantWithRelatedEntitiesWithStates(id);
            return _restaurantConverter.Convert(foundRestaurant);
        }

        public List<RestaurantResponse> GetRestaurantsResponses()
        {
            List<Restaurant> restaurants = GetRestaurantsWithCategories();
            return _restaurantConverter.Convert(restaurants);
        }

        public int CreateRestaurant(RestaurantCreate restaurantCreate)
        {
            if (!CompanyExists(restaurantCreate.CompanyId))
            {
                return -1;
            }
            Restaurant newRestaurant = _restaurantConverter.Convert(restaurantCreate);
            _context.Restaurants!.Add(newRestaurant);
            _context.SaveChanges();
            return newRestaurant.Id;
        }

        public RestaurantResponse UpdateRestaurant(int id, RestaurantCreate updatedRestaurant)
        {
            Restaurant existingRestaurant = GetRestaurantWithRelatedEntitiesWithStates(id);
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

            // Delete this restaurant's RestaurantUsers
            _restaurantUsersService.DeleteRestaurantUsers(restaurant.RestaurantUsers!);

            _context.SaveChanges();
        }

        public void DeleteRestaurantAndRelatedEntitiesById(int id)
        {
            Restaurant restaurant = GetRestaurantWithCategoriesAndRestaurantUsers(id);
            DeleteRestaurantAndRelatedEntities(restaurant);
        }

        public RestaurantDetailedResponse GetRestaurantDetailedResponse(int id)
        {
            Restaurant restaurant = GetRestaurantWithRelatedEntitiesWithStates(id);
            return _restaurantConverter.ConvertDetailed(restaurant);
        }

        public RestaurantDetailedResponse GetRestaurantMenu(int id)
        {
            Restaurant restaurant = GetRestaurantAndRelatedEntitesWithActiveState(id);
            return _restaurantConverter.ConvertDetailed(restaurant);
        }

        public List<RestaurantUserResponse> GetRestaurantUsers(int id)
        {
            Restaurant restaurant = GetRestaurantWithRestaurantUsers(id);
            
            if (restaurant.RestaurantUsers == null)
            {
                return new List<RestaurantUserResponse>();
            }
            return _restaurantUsersService.GetRestaurantUserResponses(restaurant.RestaurantUsers.ToList());
        }
    }
}
