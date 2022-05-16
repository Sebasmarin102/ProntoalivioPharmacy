using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Data.Entities
{
    public class Laboratory
    {
        public int Id { get; set; }

        [Display(Name = "Laboratorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}
