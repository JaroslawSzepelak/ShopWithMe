using ShopWithMe.Controllers;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Tests
{
    public class OrdersControllerTests
    {
        #region Can_SaveOrder()
        [Fact]
        public async Task Can_SaveOrder()
        {
            // Przygotowanie
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var contactData = new ContactData();
            var order = new Order();
            var target = new OrdersController(mock.Object, cart, contactData);

            // Działąnie
            var result = await target.Create();
            
            // Assercja
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
        }
        #endregion
    }
}
