using System;
using Qr_Menu_API.DTOs.DetailedResponseDTOs;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
	public class RestaurantDetailedResponse
	{
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? WebAddress { get; set; }
        public string Phone { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public DateTime RegisterDate { get; set; }
        public StateResponse? StateResponse { get; set; }
        public int? CompanyId { get; set; }
        public ICollection<CategoryDetailedResponse>? CategoriesDetailed{ get; set; }
    }
}

