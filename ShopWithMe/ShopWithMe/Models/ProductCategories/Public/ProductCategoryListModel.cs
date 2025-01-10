using ShopWithMe.Models.Products.Public;

namespace ShopWithMe.Models.ProductCategories.Public
{
    public class ProductCategoryListModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ProductListModel> Products { get; set; }
    }
}
