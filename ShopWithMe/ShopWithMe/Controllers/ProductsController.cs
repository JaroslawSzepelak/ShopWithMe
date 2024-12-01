using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Products;
using ShopWithMe.Models.Products;
using ShopWithMe.Models.Products.Public;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected ProductsManager _manager;
        protected ProductModelsMapper _mapper;

        #region ProductsController()
        public ProductsController(ProductsManager manager, ProductModelsMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public async Task<List<ProductListModel>> GetList()
        {
            var entries = await _manager.GetListAsync(true);

            return _mapper.MapToPublicListModel(entries);
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id, true);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.MapToDetailsModel(entity));
        }
        #endregion
    }
}
