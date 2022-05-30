using ProntoalivioPharmacy.Common;
using ProntoalivioPharmacy.Data.Entities;

namespace ProntoalivioPharmacy.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Product> Products { get; set; }
        public float Quantity { get; set; }
        public IEnumerable<MedicineType> MedicineTypes { get; set; }
    }
}
