namespace ShopWithMe.Models.Orders.Admin
{
    public class OrderFormModel
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public OrderStatus Status { get; set; }
    }
}
