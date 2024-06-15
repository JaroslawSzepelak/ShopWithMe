using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Controllers;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Tests
{
    public class OrdersControllerTests
    {
        #region Can_GetList()
        [Fact]
        public async Task Can_GetList()
        {
            // Przygotowanie
            var mock = new Mock<IOrderRepository>();
            var orders = new List<Order>()
            {
                new Order()
                {
                    Lines = new List<CartLine>()
                    {
                        new CartLine() { Product = new Product() { Name = "P1" } },
                        new CartLine() { Product = new Product() { Name = "P2" } }
                    }
                },
                new Order()
                {
                    Lines = new List<CartLine>()
                    {
                        new CartLine() { Product = new Product() { Name = "P3" } }
                    }
                }
            };

            mock.Setup(m => m.GetList()).Returns(Task.FromResult(orders.AsEnumerable()));

            var cart = new Cart();
            var contactData = new ContactData();
            var target = new OrdersController(mock.Object, cart, contactData);

            // Działąnie
            var result = await target.GetList();

            // Assercja
            Assert.IsType<OkObjectResult>(result);

            var ordersResult = ((result as OkObjectResult).Value as IEnumerable<Order>).ToList();

            Assert.Equal(2, ordersResult[0].Lines.Count);
            Assert.Equal(1, ordersResult[1].Lines.Count);
            Assert.Equal(orders[0].Lines.First().Product.Name, ordersResult[0].Lines.First().Product.Name);
            Assert.Equal(orders[1].Lines.First().Product.Name, ordersResult[1].Lines.First().Product.Name);
        }
        #endregion

        #region Can_GetSummary()
        [Fact]
        public void Can_GetSummary()
        {
            // Przygotowanie
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var product = new Product() { Name = "P1" };
            cart.AddItem(product, 1);
            var contactData = new ContactData()
            {
                Firstname = "Imię",
                Lastname = "Nazwisko",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Address = "Test 1",
                City = "Test",
                Zip = "00-000"
            };
            var target = new OrdersController(mock.Object, cart, contactData);

            // Działąnie
            var result = target.GetSummary();

            // Assercja
            Assert.IsType<OkObjectResult>(result);

            var resultModel = ((result as OkObjectResult).Value as OrderSummaryModel);

            Assert.Equal(product.Name, actual: resultModel.Lines.FirstOrDefault().Product.Name);
            Assert.Equal(contactData.Firstname, resultModel.Firstname);
            Assert.Equal(contactData.Lastname, resultModel.Lastname);
            Assert.Equal(contactData.Email, resultModel.Email);
            Assert.Equal(contactData.PhoneNumber, resultModel.PhoneNumber);
            Assert.Equal(contactData.Address, resultModel.Address);
            Assert.Equal(contactData.City, resultModel.City);
            Assert.Equal(contactData.Zip, resultModel.Zip);
        }
        #endregion

        #region Can_SaveOrder()
        [Fact]
        public async Task Can_SaveOrder()
        {
            // Przygotowanie
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var product = new Product() { Name = "P1" };
            cart.AddItem(product, 1);
            var contactData = new ContactData();
            var target = new OrdersController(mock.Object, cart, contactData);

            // Działąnie
            var result = await target.Create();
            
            // Assercja
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(product.Name, actual: ((result as OkObjectResult).Value as Order).Lines.FirstOrDefault().Product.Name);
        }
        #endregion

        #region Can_Delete_Order()
        [Fact]
        public async Task Can_Delete_Order()
        {
            // Przygotowanie
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var contactData = new ContactData();
            var target = new OrdersController(mock.Object, cart, contactData);

            // Działąnie
            var result = await target.Delete(1);

            // Assercja
            mock.Verify(m => m.Delete(It.IsAny<long>()), Times.Once);
            Assert.IsType<OkResult>(result);
        }
        #endregion
    }
}
