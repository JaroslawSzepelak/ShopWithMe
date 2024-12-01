using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers;
using ShopWithMe.Models.Admin;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
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

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategory product)
        {
            var entity = product;

            await _manager.CreateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductCategory product)
        {
            var entity = product;

            await _manager.UpdateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region Delete()
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _manager.GetAsync(id);

            if (entity == null)
                return NotFound();

            await _manager.DeleteAsync(entity);

            return Ok();
        }
        #endregion
    }
}
