using ShopWithMe.Models.Products;

namespace ShopWithMe.Models.Interfaces
{
    public interface IStoreRepository
    {
        public IQueryable<Product> Products { get; }
    }
}
