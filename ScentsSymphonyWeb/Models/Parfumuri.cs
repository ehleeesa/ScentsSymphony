using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScentsSymphonyWeb.Models
{
    public class Parfumuri
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(1000, ErrorMessage = "Descrierea nu poate depăși 1000 de caractere.")]
        public string Description { get; set; }

        [Range(0, 50000, ErrorMessage = "Prețul trebuie să fie între 0 și 50000 RON")]
        public int? Price50ml { get; set; }

        [Range(0, 50000, ErrorMessage = "Prețul trebuie să fie între 0 și 50000 RON")]
        public int? Price100ml { get; set; }

        [Required(ErrorMessage = "The Perfume notes field is required.")]
        [StringLength(500, ErrorMessage = "Notele de parfum nu pot depăși 500 de caractere.")]
        public string PerfumeNotes { get; set; }

        [Required(ErrorMessage = "The Stock value is invalid.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stocul nu poate fi negativ.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "The Image URL field is required.")]
        [StringLength(255, ErrorMessage = "URL-ul imaginii nu poate depăși 255 de caractere.")]
        [Url(ErrorMessage = "URL-ul imaginii nu este valid.")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Câmpul Brand este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Brandul nu poate depăși 100 de caractere.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Câmpul Nișă este obligatoriu.")]
        [StringLength(50)]
        public string Nisa { get; set; }

        [Required(ErrorMessage = "Câmpul Cantitate disponibilă este obligatoriu.")]
        [StringLength(50)]
        public string CantitateDisponibila { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public Parfumuri()
        {
            Reviews = new List<Review>();
        }
    }
}
