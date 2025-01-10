using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.Storage;
using ShopWithMe.Tools.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWithMe.Models.Products
{
    public class Product : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Lead {  get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        [ForeignKey("ProductCategory")]
        public long? CategoryId { get; set; }
        public string TechnicalData { get; set; }
        [ForeignKey("StorageFile")]
        public long? MainImageId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? SalePrice { get; set; }
        public DateTime? DateSaleFrom { get; set; }
        public DateTime? DateSaleTo { get; set; }
        public bool IsSaleOn {  get; set; }


        public virtual ProductCategory Category { get; set; }
        public virtual StorageFile MainImage { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
