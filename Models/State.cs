using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.Models
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Id { get; set; }

        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; } = "";

        public virtual ICollection<ApplicationUser>? ApplicationUsers { get; set; }
        public virtual ICollection<Company>? Companies { get; set; }
        public virtual ICollection<Restaurant>? Restaurants { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Food>? Foods { get; set; }

        public State(byte id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
