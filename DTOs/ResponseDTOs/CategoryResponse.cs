using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; } = "";

        public int RestaurantId { get; set; }

        public StateResponse? StateResponse { get; set; }

        public virtual ICollection<int>? FoodIds { get; set; }
    }
}
