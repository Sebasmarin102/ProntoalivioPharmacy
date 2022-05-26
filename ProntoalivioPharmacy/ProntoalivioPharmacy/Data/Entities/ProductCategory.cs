namespace ProntoalivioPharmacy.Data.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public MedicineType MedicineType { get; set; }
    }
}
