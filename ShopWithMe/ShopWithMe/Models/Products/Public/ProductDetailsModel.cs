﻿using ShopWithMe.Models.Storage;

namespace ShopWithMe.Models.Products.Public
{
    public class ProductDetailsModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Lead { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public string Category { get; set; }
        public string TechnicalData { get; set; }
        public StorageFile MainImage { get; set; }
        public List<StorageFile> Images { get; set; }
    }
}
