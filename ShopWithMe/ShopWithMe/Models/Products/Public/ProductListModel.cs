using ShopWithMe.Models.Storage;

namespace ShopWithMe.Models.Products.Public
{
    public class ProductListModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Lead { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public string Category { get; set; }
        public StorageFile MainImage { get; set; }
    }
}
