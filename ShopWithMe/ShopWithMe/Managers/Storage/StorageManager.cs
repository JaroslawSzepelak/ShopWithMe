using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Storage;

namespace ShopWithMe.Managers.Storage
{
    public class StorageManager : BaseManager<StorageFile>
    {
        #region StorageManager
        public StorageManager(IBaseRepository<StorageFile> repository) : base(repository) { }
        #endregion

        #region GetAsync()
        public virtual async Task<StorageFile> GetAsync(string fileName)
        {
            return await _repository.Entities.FirstOrDefaultAsync(f => f.Name == fileName);
        }
        #endregion

    }
}
