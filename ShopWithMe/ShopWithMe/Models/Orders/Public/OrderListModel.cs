namespace ShopWithMe.Models.Orders.Public
{
    public class OrderListModel
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
