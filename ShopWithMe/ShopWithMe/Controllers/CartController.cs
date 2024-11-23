using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Cart.FormModels;

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

        #region UpdateCartLine()
        [HttpPut]
        public IActionResult UpdateCartLine([FromBody] CartLineUpdateMOdel model)
        {
            var cartLine = _cart.Lines.First(c1 => c1.Product.Id == model.ProductId);

            if (cartLine == null)
                return NotFound();

            cartLine.Quantity = model.Quantity;

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
            var product = _cart.Lines.FirstOrDefault(c1 => c1.Product.Id == productId)?.Product;

            if (product == null)
                return NotFound();

            _cart.RemoveLine(product);
            return Ok(_cart);
        }
        #endregion
    }
}
