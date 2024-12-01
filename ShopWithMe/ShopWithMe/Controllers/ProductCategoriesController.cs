using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        protected ProductCategoriesManager _manager;

        #region ProductCategoriesController()
        public ProductCategoriesController(ProductCategoriesManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public async Task<List<ProductCategory>> GetList()
        {
            return await _manager.GetListAsync();
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }
        #endregion
    }
}
