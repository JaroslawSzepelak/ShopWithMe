namespace ShopWithMe.Models.Cart.FormModels
{
    public class CartLineUpdateModel
    {
        public long ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
