using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult UpdateCartLine([FromBody] CartLineUpdateModel model)
        {
            var cartLine = _cart.Lines.FirstOrDefault(c1 => c1.Product.Id == model.ProductId);

            if (cartLine == null)
                return NotFound();

            _cart.UpdateItem(cartLine, model.Quantity);

            return Ok(_cart);
        }
        #endregion

        #region AddItem()
        [HttpPost]
        public IActionResult AddItem([FromBody] CartLineUpdateModel model)
        {
            var product = _context.Products.Include(p => p.MainImage).FirstOrDefault(p => p.Id == model.ProductId);

            if (product == null)
                return NotFound();

            _cart.AddItem(product, model.Quantity ?? 1);
            return Ok(_cart);
        }
        #endregion

        #region RemoveLine()
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

        #region Clear()
        [HttpDelete("clear")]
        public IActionResult Clear()
        {
            _cart.Clear();

            return Ok(_cart);
        }
        #endregion
    }
}
