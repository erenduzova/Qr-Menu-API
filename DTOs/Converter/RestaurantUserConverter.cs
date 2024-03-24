using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.DTOs.Converter
{
    public class RestaurantUserConverter
    {
        public RestaurantUser Convert(RestaurantUserCreate restaurantUserCreate)
        {
            RestaurantUser newRestaurantUser = new()
            {
                RestaurantId = restaurantUserCreate.RestaurantId,
                UserId = restaurantUserCreate.UserId,
            };
            return newRestaurantUser;
        }

        public RestaurantUserResponse Convert(RestaurantUser restaurantUser)
        {
            RestaurantUserResponse restaurantUserResponse = new()
            {
                RestaurantId = restaurantUser.RestaurantId,
                UserId = restaurantUser.UserId,
            };
            return restaurantUserResponse;
        }

        public List<RestaurantUserResponse> Convert(List<RestaurantUser> restaurantUsers)
        {
            List<RestaurantUserResponse> restaurantUserResponses = new List<RestaurantUserResponse>();
            foreach (RestaurantUser restaurantUser in restaurantUsers)
            {
                restaurantUserResponses.Add(Convert(restaurantUser));
            }
            return restaurantUserResponses;
        }
    }
}
