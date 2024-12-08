using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Tools.Interfaces;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Managers
{
    public class BaseManager<TEntity> where TEntity : class, IEntity
    {
        protected readonly IBaseRepository<TEntity> _repository;

        #region ProductsManagers()
        public BaseManager(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }
        #endregion

        #region GetListAsync()
        public virtual async Task<List<TEntity>> GetListAsync()
        {
            return await GetListAsync((Pager)null);
        }

        public virtual async Task<List<TEntity>> GetListAsync(Pager pager)
        {
            var query = _repository.Entities.AsQueryable();

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
        public virtual async Task<TEntity> GetAsync(long id)
        {
            return await _repository.FindAsync(id);
        }
        #endregion

        #region CreateAsync()
        public virtual async Task CreateAsync(TEntity product)
        {
            await _repository.CreateAsync(product);
        }
        #endregion

        #region UpdateAsync()
        public virtual async Task UpdateAsync(TEntity product)
        {
            await _repository.UpdateAsync(product);
        }
        #endregion

        #region DeleteAsync()
        public virtual async Task DeleteAsync(TEntity product)
        {
            await _repository.DeleteAsync(product);
        }
        #endregion
    }
}
