using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Managers
{
    public class ProductsManager : BaseManager<Product>
    {
        #region ProductsManagers()
        public ProductsManager(IBaseRepository<Product> repository) : base(repository) { }
        #endregion

        #region GetAsync()
        public override async Task<Product> GetAsync(long id)
        {
            return await GetAsync(id, false);
        }

        public async Task<Product> GetAsync(long id, bool loadLinkedData)
        {
            var query = _repository.Entities.AsQueryable();

            if (loadLinkedData)
            {
                query = query.Include(q => q.Category);
            }

            return await query.FirstOrDefaultAsync(q => q.Id == id);
        }
        #endregion
    }
}
