﻿using ProntoalivioPharmacy.Data.Entities;

namespace ProntoalivioPharmacy.Models
{
    public class HomeViewModel
    {
        public ICollection<Product> Products { get; set; }
        public float Quantity { get; set; }
    }
}
