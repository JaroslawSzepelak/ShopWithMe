using ShopWithMe.Tools.Interfaces;

namespace ShopWithMe.Models.Common
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        public IQueryable<TEntity> Entities { get; }

        TEntity Find(long id);
        Task<TEntity> FindAsync(long id);
        void Create(TEntity entity);
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
