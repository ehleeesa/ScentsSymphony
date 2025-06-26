using System.ComponentModel.DataAnnotations;

namespace ScentsSymphonyWeb.Models
{
    public class ShippingInfo
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Adresa este obligatorie")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Județul este obligatoriu")]
        public string County { get; set; }

        [Required(ErrorMessage = "Orașul este obligatoriu")]
        public string City { get; set; }

        [Required(ErrorMessage = "Codul poștal este obligatoriu")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Metoda de livrare este obligatorie")]
        [Display(Name = "Metodă de livrare")]
        public string DeliveryMethod { get; set; }
    }
}
