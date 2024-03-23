using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = "";

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? WebAddress { get; set; }

        [Phone]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string Phone { get; set; } = "";

        [StringLength(5, MinimumLength = 5)]
        [Column(TypeName = "char(5)")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; } = "";

        [StringLength(200, MinimumLength = 5)]
        [Column(TypeName = "nvarchar(200)")]
        public string AddressDetails { get; set; } = "";

        [Column(TypeName = "smalldatetime")]
        public DateTime RegisterDate { get; set; }
        
        public byte StateId { get; set; }
        [ForeignKey("StateId")]
        public State? State { get; set; }
        
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<RestaurantUser>? Users { get; set; }
    }
}
