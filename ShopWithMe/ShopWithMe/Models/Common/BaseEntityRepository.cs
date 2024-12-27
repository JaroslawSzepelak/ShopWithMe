using Microsoft.EntityFrameworkCore;
using ShopWithMe.Tools.Abstractions;

namespace ShopWithMe.Models.Common
{
    /// <summary>
    /// Bazowa klasa odpowiedzialna za operacje na bazie danych
    /// </summary>
    /// <typeparam name="TEntity">Klasa modelu danych używanych w DefaultContext, implementująca interfejs IEntity</typeparam>
    public class BaseEntityRepository<TEntity> : BaseRepository<TEntity>, IBaseEntityRepository<TEntity> where TEntity : class, IEntity
    {
        #region BaseEntityRepository()
        public BaseEntityRepository(DefaultContext context) : base(context) { }
        #endregion

        #region Find()
        public virtual TEntity Find(long id)
        {
            return Entities.FirstOrDefault(e => e.Id == id);
        }
        #endregion

        #region FindAsync()
        public virtual async Task<TEntity> FindAsync(long id)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Id == id);
        }
        #endregion
    }
}
