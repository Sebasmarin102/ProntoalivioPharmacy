using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Data.Entities
{
    public class Neighborhood
    {
        public int Id { get; set; }

        [Display(Name = "Barrio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public City City { get; set; }
        public ICollection<Laboratory> Laboratories { get; set; }

        [Display(Name = "Laboratorios")]
        public int LaboratoriesNumber => Laboratories == null ? 0 : Laboratories.Count;
    }
}
