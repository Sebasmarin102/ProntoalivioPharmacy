using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Data.Entities
{
    public class MedicineType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de medicamento")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
    }
}
