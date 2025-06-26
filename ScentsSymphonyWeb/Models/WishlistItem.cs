using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ScentsSymphonyWeb.Models
{
    public class WishlistItem
    {
        [Key]
        public int WishlistItemID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey("UserID")]
        public virtual IdentityUser User { get; set; }

        [ForeignKey("ProductID")]
        public virtual Parfumuri Product { get; set; }
    }
}
