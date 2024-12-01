using Microsoft.EntityFrameworkCore;
using ShopWithMe.Tools.Interfaces;

namespace ShopWithMe.Models.Common
{
    /// <summary>
    /// Bazowa klasa odpowiedzialna za operacje na bazie danych
    /// </summary>
    /// <typeparam name="TEntity">Klasa modelu danych używanych w DefaultContext, implementująca interfejs IEntity</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected DefaultContext _context;
        public IQueryable<TEntity> Entities => _context.Set<TEntity>();

        #region BaseRepository()
        public BaseRepository(DefaultContext context)
        {
            _context = context;
        }
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

        #region Create()
        public virtual void Create(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        #endregion

        #region CreateAsync()
        public virtual async Task CreateAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Update()
        public virtual void Update(TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        #endregion

        #region UpdateAsync()
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete()
        public virtual void Delete(TEntity entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        #endregion

        #region DeleteAsync()
        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
