using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Managers.Products
{
    public class ProductImagesManager : BaseManager<ProductImage>
    {
        #region ProductImagesManager()
        public ProductImagesManager(IBaseRepository<ProductImage> repository) : base(repository) { }
        #endregion

        #region GetAsync()
        public async Task<ProductImage> GetAsync(long productId, long imageId)
        {
            return await _repository.Entities.FirstOrDefaultAsync(p => p.ProductId == productId && p.StorageFileId == imageId);
        }
        #endregion
    }
}
