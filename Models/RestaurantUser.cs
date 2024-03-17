using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.Models
{
    public class RestaurantUser
    {
       
        public int RestaurantId { get; set; }
        public string UserId { get; set; } = "";


        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
