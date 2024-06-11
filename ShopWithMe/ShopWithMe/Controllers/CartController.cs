using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models;
using ShopWithMe.Models.Cart;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        protected DefaultContext _context;
        protected Cart _cart;

        #region CartController()
        public CartController(DefaultContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }
        #endregion

        #region Get()
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_cart);
        }
        #endregion

        #region OnPost()
        [HttpPost]
        public IActionResult AddItem([FromBody] long productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return NotFound();

            _cart.AddItem(product, 1);
            return Ok(_cart);
        }
        #endregion

        #region OnPostRemove()
        [HttpDelete]
        public IActionResult RemoveLine([FromBody] long productId)
        {
            _cart.RemoveLine(_cart.Lines.First(c1 => c1.Product.Id == productId).Product);
            return Ok(_cart);
        }
        #endregion
    }
}
