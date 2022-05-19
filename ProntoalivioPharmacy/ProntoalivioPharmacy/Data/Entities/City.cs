using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public ICollection<Neighborhood> Neighborhoods { get; set; }

        [Display(Name = "Barrios")]
        public int NeighborhoodsNumber => Neighborhoods == null ? 0 : Neighborhoods.Count;

        public ICollection<User> Users { get; set; }
    }
}
