using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected ProductsManager _manager;

        #region ProductsController()
        public ProductsController(ProductsManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public async Task<List<Product>> GetList()
        {
            return await _manager.GetListAsync();
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id, true);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }
        #endregion
    }
}
