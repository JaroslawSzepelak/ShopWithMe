using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Abstractions;

namespace ShopWithMe.Models.Cart
{
    public class CartLine : IEntity
    {
        public long Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
