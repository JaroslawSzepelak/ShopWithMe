using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected DefaultContext _context;

        #region ProductsController()
        public ProductsController(DefaultContext context) 
        {
            _context = context;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public IAsyncEnumerable<Product> GetList()
        {
            return _context.Products.AsAsyncEnumerable();
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var entity = product;

            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Product product)
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
            var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
