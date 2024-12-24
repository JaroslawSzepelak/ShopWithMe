using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopWithMe.Models.ProductCategories;
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


        public virtual ProductCategory Category { get; set; }
    }
}
