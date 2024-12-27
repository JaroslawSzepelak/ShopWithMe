using ShopWithMe.Tools.Abstractions;

namespace ShopWithMe.Models.Common
{
    public interface IBaseEntityRepository<TEntity>: IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Find(long id);
        Task<TEntity> FindAsync(long id);
    }
}
