using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.Products;
using Microsoft.AspNetCore.Authorization;
using ShopWithMe.Models.Admin;
using ShopWithMe.Managers;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
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

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var entity = product;

            await _manager.CreateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Product product)
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
