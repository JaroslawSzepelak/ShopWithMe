using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopWithMe.Tools.Interfaces;
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
    }
}
