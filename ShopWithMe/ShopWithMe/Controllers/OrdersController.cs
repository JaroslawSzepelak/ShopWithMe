using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        protected DefaultContext _context;
        protected Cart _cart;
        protected ContactData _contactData;

        #region OrdersController()
        public OrdersController(DefaultContext context, Cart cart, ContactData contactData)
        {
            _context = context;
            _cart = cart;
            _contactData = contactData;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public IAsyncEnumerable<Order> GetList()
        {
            return _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .AsAsyncEnumerable();
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

            _context.AttachRange(entity.Lines.Select(l => l.Product));

            if (entity.Id == 0)
            {
                await _context.Orders.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            return Ok(entity);
        }
        #endregion

        #region Delete()
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
