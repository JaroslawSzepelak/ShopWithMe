using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.ProductCategories;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.ProductCategories.Public;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        protected ProductCategoriesManager _manager;
        protected ProductCategoryModelsMapper _mapper;

        #region ProductCategoriesController()
        public ProductCategoriesController(ProductCategoriesManager manager, ProductCategoryModelsMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        #endregion

        #region GetList()
        [HttpGet]
        public async Task<List<ProductCategoryListModel>> GetList()
        {
            var entries = await _manager.GetListAsync();

            return _mapper.MapToPublicListModel(entries);
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.MapToDetailsModel(entity));
        }
        #endregion
    }
}
