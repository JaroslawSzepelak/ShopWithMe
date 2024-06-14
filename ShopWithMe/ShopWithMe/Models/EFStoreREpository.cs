using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Interfaces;

namespace ShopWithMe.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly DefaultContext _context;

        public EFStoreRepository(DefaultContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;
    }
}
