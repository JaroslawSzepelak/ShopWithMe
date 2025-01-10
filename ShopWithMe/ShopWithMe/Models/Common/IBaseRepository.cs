namespace ShopWithMe.Models.Common
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Entities { get; }

        void Create(TEntity entity);
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
