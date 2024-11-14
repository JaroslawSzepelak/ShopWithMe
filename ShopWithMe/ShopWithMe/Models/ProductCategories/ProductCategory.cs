using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Interfaces;

namespace ShopWithMe.Models.ProductCategories
{
    public class ProductCategory : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
