using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Models
{
    public class CreateProductViewModel : EditProductViewModel
    {
        [Display(Name = "Tipo de medicina")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de medicina.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int MedicineTypeId { get; set; }

        public IEnumerable<SelectListItem> MedicineTypes { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }

    }
}
