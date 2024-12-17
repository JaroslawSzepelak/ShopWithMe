using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Managers.Products
{
    public class ProductsManager : BaseManager<Product>
    {
        #region ProductsManagers()
        public ProductsManager(IBaseRepository<Product> repository) : base(repository) { }
        #endregion

        #region GetListAsync()
        public override async Task<List<Product>> GetListAsync()
        {
            return await GetListAsync(false);
        }
        
        public async Task<List<Product>> GetListAsync(bool loadLinkedData)
        {
            var query = _repository.Entities.AsQueryable();

            if (loadLinkedData)
            {
                query = query.Include(q => q.Category);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetListAsync(string search)
        {
            return await _repository.Entities
                .Where(p => p.Name.Contains(search))
                .Take(10)
                .ToListAsync();
        }

        public override async Task<List<Product>> GetListAsync(Pager pager)
        {
            return await GetListAsync(false, pager);
        }

        public async Task<List<Product>> GetListAsync(bool loadLinkedData, Pager pager)
        {
            var query = _repository.Entities.AsQueryable();

            if (loadLinkedData)
            {
                query = query.Include(q => q.Category);
            }

            if (pager != null)
            {
                pager.TotalRows = query.Count();

                query = query
                    .Skip(pager.Skip)
                    .Take(pager.PageSize);
            }

            return await query.ToListAsync();
        }
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
