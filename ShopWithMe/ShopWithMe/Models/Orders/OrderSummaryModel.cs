using ShopWithMe.Models.Cart;

namespace ShopWithMe.Models.Orders
{
    public class OrderSummaryModel
    {
        public ICollection<CartLine> Lines { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }

        #region OrderSummaryModel()
        public OrderSummaryModel(Cart.Cart cart, ContactData.ContactData contactData)
        {
            Lines = cart.Lines;
            Firstname = contactData.Firstname;
            Lastname = contactData.Lastname;
            Email = contactData.Email;
            PhoneNumber = contactData.PhoneNumber;
            Address = contactData.Addrress;
            City = contactData.City;
            Zip = contactData.Zip;
        }
        #endregion
    }
}
