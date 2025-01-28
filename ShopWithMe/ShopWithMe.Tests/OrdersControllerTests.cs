using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ShopWithMe.Controllers;
using ShopWithMe.Managers.Orders;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Orders.Public;
using ShopWithMe.Tools.Models;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Tests
{
    public class OrdersControllerTests
    {/*
        private Mock<IOrderRepository> _mockRepository;
        private Mock<OrdersManager> _mockManager;
        private Mock<OrderModelsMapper> _mockMapper;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Cart _cart;
        private ContactData _contactData;

        public OrdersControllerTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _mockManager = new Mock<OrdersManager>();
            _mockMapper = new Mock<OrderModelsMapper>();
            _cart = new Cart();
            _contactData = new ContactData();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null
            );
        }

        #region Can_GetList()
        [Fact]
        public async Task Can_GetList()
        {
            // Przygotowanie
            var orders = new List<Order>
            {
                new Order
                {
                    Lines = new List<CartLine>
                    {
                        new CartLine { Product = new Product { Name = "P1" } },
                        new CartLine { Product = new Product { Name = "P2" } }
                    }
                },
                new Order
                {
                    Lines = new List<CartLine>
                    {
                        new CartLine { Product = new Product { Name = "P3" } }
                    }
                }
            };

            _mockManager.Setup(m => m.GetListAsync(It.IsAny<string>(), It.IsAny<Pager>()))
                .ReturnsAsync(orders);

            _mockMapper.Setup(m => m.MapToPublicListModel(It.IsAny<List<Order>>()))
                .Returns(orders.Select(o => new OrderListModel()).ToList());


            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser { Id = "user-id" });

            var target = new OrdersController(_mockRepository.Object, _mockManager.Object, _mockMapper.Object, _cart, _contactData, _mockUserManager.Object);

            // Działanie
            var result = await target.GetList(1, 10);

            // Assercja
            Assert.IsType<OkObjectResult>(result);
            _mockManager.Verify(m => m.GetListAsync("user-id", It.IsAny<Pager>()), Times.Once);
        }
        #endregion

        #region Can_GetSummary()
        [Fact]
        public void Can_GetSummary()
        {
            // Przygotowanie
            var product = new Product { Name = "P1", Price = 100M, SalePrice = 90M };
            _cart.AddItem(product, 1);

            _contactData.Firstname = "Imię";
            _contactData.Lastname = "Nazwisko";
            _contactData.Email = "test@example.com";
            _contactData.PhoneNumber = "123456789";
            _contactData.Address = "Test 1";
            _contactData.City = "Test";
            _contactData.Zip = "00-000";

            var target = new OrdersController(_mockRepository.Object, _mockManager.Object, _mockMapper.Object, _cart, _contactData, _mockUserManager.Object);

            // Działanie
            var result = target.GetSummary();

            // Assercja
            Assert.IsType<OkObjectResult>(result);
            var resultModel = ((result as OkObjectResult).Value as OrderSummaryModel);

            Assert.Equal(product.Name, resultModel.Lines.FirstOrDefault().Product.Name);
            Assert.Equal(_contactData.Firstname, resultModel.Firstname);
            Assert.Equal(_contactData.Lastname, resultModel.Lastname);
        }
        #endregion

        #region Can_SaveOrder()
        [Fact]
        public async Task Can_SaveOrder()
        {
            // Przygotowanie
            var product = new Product { Name = "P1", Price = 100M, SalePrice = 90M };
            _cart.AddItem(product, 1);

            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser { Id = "user-id" });

            var target = new OrdersController(_mockRepository.Object, _mockManager.Object, _mockMapper.Object, _cart, _contactData, _mockUserManager.Object);

            // Działanie
            var result = await target.Create();

            // Assercja
            _mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
        #endregion
        */
    }
}
