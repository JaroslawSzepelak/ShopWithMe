using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Tools.Exceptions;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        protected IOrderRepository _repository;
        protected Cart _cart;
        protected ContactData _contactData;

        #region OrdersController()
        public OrdersController(IOrderRepository repository, Cart cart, ContactData contactData)
        {
            _repository = repository;
            _cart = cart;
            _contactData = contactData;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _repository.GetList());
        }
        #endregion

        #region GetSummary()
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            return Ok(new OrderSummaryModel(_cart, _contactData));
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var entity = new Order(_cart, _contactData);

            await _repository.SaveOrder(entity);

            return Ok(entity);
        }
        #endregion

        #region Delete()
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.Delete(id);

                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
