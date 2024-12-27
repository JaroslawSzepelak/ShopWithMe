using ShopWithMe.Models.Storage;

namespace ShopWithMe.Models.Products
{
    public class ProductImage
    {
        public long ProductId { get; set; }
        public long StorageFileId { get; set; }

        public Product Product { get; set; }
        public StorageFile StorageFile { get; set; }
    }
}
