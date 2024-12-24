using ShopWithMe.Tools.Abstractions;

namespace ShopWithMe.Models.Storage
{
    public class StorageFile : BaseEntity, IAuditable
    {
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
