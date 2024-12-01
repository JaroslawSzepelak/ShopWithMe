namespace ShopWithMe.Models.Products
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        Task<IEnumerable<Product>> GetList();
        Task SaveProduct(Product Product);
        Task Delete(long id);
    }
}
