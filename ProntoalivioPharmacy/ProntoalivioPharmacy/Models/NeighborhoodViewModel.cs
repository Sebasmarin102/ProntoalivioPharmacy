using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Models
{
    public class NeighborhoodViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Barrio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public int CityId { get; set; }
    }
}
