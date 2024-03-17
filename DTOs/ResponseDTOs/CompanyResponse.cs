using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
    public class CompanyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string PostalCode { get; set; } = "";

        public string AddressDetails { get; set; } = "";

        public string Phone { get; set; } = "";

        public string EMail { get; set; } = "";
        public DateTime RegisterDate { get; set; }

        public string TaxNumber { get; set; } = "";

        public string? WebAddress { get; set; }

        public StateResponse? StateResponse { get; set; }

        public ICollection<int>? RestaurantIds { get; set; }
    }
}
