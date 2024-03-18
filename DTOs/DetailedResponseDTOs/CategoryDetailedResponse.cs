using System;
using Qr_Menu_API.DTOs.ResponseDTOs;

namespace Qr_Menu_API.DTOs.DetailedResponseDTOs
{
	public class CategoryDetailedResponse
	{
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; } = "";

        public int RestaurantId { get; set; }

        public StateResponse? StateResponse { get; set; }

        public virtual ICollection<FoodResponse>? FoodsDetailed { get; set; }
    }
}

