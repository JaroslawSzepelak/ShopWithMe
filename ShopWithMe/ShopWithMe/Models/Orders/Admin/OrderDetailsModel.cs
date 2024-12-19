using ShopWithMe.Models.Cart;

namespace ShopWithMe.Models.Orders.Admin
{
    public class OrderDetailsModel
    {
        public long Id { get; set; }
        public ICollection<CartLine> Lines { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
    }
}
