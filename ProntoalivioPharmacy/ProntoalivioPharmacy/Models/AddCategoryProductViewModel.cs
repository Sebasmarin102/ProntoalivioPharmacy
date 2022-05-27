using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Models
{
    public class AddCategoryProductViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Tipo medicamento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo medicamento.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int MedicineTypeId { get; set; }
        public IEnumerable<SelectListItem> MedicineTypes { get; set; }
    }
}
