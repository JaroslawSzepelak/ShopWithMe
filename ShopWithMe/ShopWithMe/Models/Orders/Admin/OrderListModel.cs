namespace ShopWithMe.Models.Orders.Admin
{
    public class OrderListModel
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
