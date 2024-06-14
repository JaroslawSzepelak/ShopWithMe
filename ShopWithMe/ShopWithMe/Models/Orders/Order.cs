using ShopWithMe.Models.Cart;
using ShopWithMe.Tools.Interfaces;

namespace ShopWithMe.Models.Orders
{
    public class Order : IEntity
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

        #region Order()
        public Order() { }

        public Order(Cart.Cart cart, ContactData.ContactData contactData)
        {
            Lines = cart.Lines;
            Firstname = contactData.Firstname;
            Lastname = contactData.Lastname;
            Email = contactData.Email;
            PhoneNumber = contactData.PhoneNumber;
            Address = contactData.Address;
            City = contactData.City;
            Zip = contactData.Zip;
        }
        #endregion
    }
}
