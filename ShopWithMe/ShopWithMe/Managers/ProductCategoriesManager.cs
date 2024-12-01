using ShopWithMe.Models.Common;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Managers
{
    public class ProductCategoriesManager : BaseManager<ProductCategory>
    {
        #region ProductsManagers()
        public ProductCategoriesManager(IBaseRepository<ProductCategory> repository) : base(repository) { }
        #endregion
    }
}
