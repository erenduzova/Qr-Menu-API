using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public override string? UserName { get; set; } = "";

        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; } = "";

        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        [Column(TypeName = "varchar(100)")]
        public override string? Email { get; set; } = "";

        [Phone]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public override string? PhoneNumber { get; set; } = "";

        [Column(TypeName = "smalldatetime")]
        public DateTime RegisterDate { get; set; }
        
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
        
        public byte StateId { get; set; }
        [ForeignKey("StateId")]
        public State? State { get; set; }

        public virtual ICollection<Restaurant>? Restaurants { get; set; }
    }
}
