using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.ProductCategories;
using ShopWithMe.Models.Admin;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.ProductCategories.Admin;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
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
        public async Task<ResultModel<ProductCategoryListModel>> GetList([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var pager = new Pager()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var entries = await _manager.GetListAsync(pager);

            return new ResultModel<ProductCategoryListModel>(_mapper.MapToAdminListModel(entries), pager);
        }

        [HttpGet("all")]
        public async Task<List<ProductCategoryListModel>> GetList()
        {
            var entries = await _manager.GetListAsync();

            return _mapper.MapToAdminListModel(entries);
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.MapToFormModel(entity));
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryFormModel model)
        {
            var entity = new ProductCategory();

            _mapper.Map(model, entity);

            await _manager.CreateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductCategoryFormModel model)
        {
            var entity = await _manager.GetAsync(model.Id);

            if (entity == null)
                return NotFound();

            _mapper.Map(model, entity);

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
