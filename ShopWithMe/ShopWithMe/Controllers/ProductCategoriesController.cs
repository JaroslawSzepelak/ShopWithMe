using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models;
using ShopWithMe.Models.ProductCategories;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        protected DefaultContext _context;

        #region ProductCategoriesController()
        public ProductCategoriesController(DefaultContext context) 
        {
            _context = context;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public IAsyncEnumerable<ProductCategory> GetList()
        {
            return _context.ProductCategories.AsAsyncEnumerable();
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategory product)
        {
            var entity = product;

            await _context.ProductCategories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductCategory product)
        {
            var entity = product;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }
        #endregion

        #region Delete()
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
