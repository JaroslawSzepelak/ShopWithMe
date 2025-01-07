using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Managers.Products
{
    public class ProductsManager : BaseEntityManager<Product>
    {
        #region ProductsManagers()
        public ProductsManager(IBaseEntityRepository<Product> repository) : base(repository) { }
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
                query = query
                    .Include(q => q.Category)
                    .Include(p => p.MainImage);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetListAsync(string search)
        {
            return await _repository.Entities
                .Where(p => p.Name.Contains(search))
                .Include(p => p.MainImage)
                .Take(10)
                .ToListAsync();
        }

        public override async Task<List<Product>> GetListAsync(Pager pager)
        {
            return await GetListAsync(false, pager);
        }

        public async Task<List<Product>> GetListAsync(bool loadLinkedData, Pager pager)
        {
            return await GetListAsync(false, pager, 0, null);
        }

        public async Task<List<Product>> GetListAsync(bool loadLinkedData, Pager pager, long categoryId, string search)
        {
            var query = _repository.Entities.AsQueryable();

            if (loadLinkedData)
            {
                query = query
                    .Include(q => q.Category)
                    .Include(p => p.MainImage);
            }

            if (categoryId > 0)
            {
                query = query
                    .Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query
                    .Where(p => p.Name.Contains(search));
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
                query = query
                    .Include(q => q.Category)
                    .Include(p => p.MainImage)
                    .Include(p => p.Images)
                        .ThenInclude(i => i.StorageFile);
            }

            return await query.FirstOrDefaultAsync(q => q.Id == id);
        }
        #endregion
    }
}
