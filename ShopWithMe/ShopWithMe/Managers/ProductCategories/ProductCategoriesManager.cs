using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Managers.ProductCategories
{
    public class ProductCategoriesManager : BaseEntityManager<ProductCategory>
    {
        #region ProductsManagers()
        public ProductCategoriesManager(IBaseEntityRepository<ProductCategory> repository) : base(repository) { }
        #endregion

        #region GetListAsync()
        public virtual async Task<List<ProductCategory>> GetListAsync(bool loadWithProducts)
        {
            var query = _repository.Entities.AsQueryable();

            if (loadWithProducts == true)
            {
                query = query.Include(q => q.Products.Take(10));
            }

            return await query.ToListAsync();
        }
        #endregion
    }
}
