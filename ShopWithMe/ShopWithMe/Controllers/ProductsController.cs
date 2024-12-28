using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Products;
using ShopWithMe.Models.Products;
using ShopWithMe.Models.Products.Public;
using ShopWithMe.Tools.Models;

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
        public async Task<ResultModel<ProductListModel>> GetList(
            [FromQuery] int pageIndex, 
            [FromQuery] int pageSize,
            [FromQuery] int categoryId,
            [FromQuery] string search)
        {
            var pager = new Pager()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var entries = await _manager.GetListAsync(true, pager, categoryId, search);

            return new ResultModel<ProductListModel>(_mapper.MapToPublicListModel(entries), pager);
        }
        #endregion

        #region GetAutocomplete()
        [HttpGet("get-autocomplete")]
        public async Task<List<ProductAutocompleteModel>> GetAutocomplete([FromQuery] string search)
        {
            var entries = await _manager.GetListAsync(search);

            return _mapper.MapToPublicAutocompleteModel(entries);
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
