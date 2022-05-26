using System.ComponentModel.DataAnnotations;

namespace ProntoalivioPharmacy.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7237/images/noimage.png"
            : $"https://prontoalivio.blob.core.windows.net/products/{ImageId}";

    }
}
