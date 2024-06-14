using ShopWithMe.Models.Products;

namespace ShopWithMe.Tools.Interfaces
{
    public interface IStoreRepository
    {
        public IQueryable<Product> Products { get; }
    }
}
