using ShopWithMe.Models.Common;
using ShopWithMe.Models.Storage;

namespace ShopWithMe.Managers.Storage
{
    public class StorageManager : BaseManager<StorageFile>
    {
        #region StorageManager
        public StorageManager(IBaseRepository<StorageFile> repository) : base(repository) { }
        #endregion
    }
}
