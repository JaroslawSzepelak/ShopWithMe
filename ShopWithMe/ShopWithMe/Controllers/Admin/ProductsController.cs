using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.Products;
using Microsoft.AspNetCore.Authorization;
using ShopWithMe.Models.Admin;
using ShopWithMe.Managers.Products;
using ShopWithMe.Models.Products.Admin;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
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
        public async Task<ResultModel<ProductListModel>> GetList([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var pager = new Pager()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var entries = await _manager.GetListAsync(true, pager);

            return new ResultModel<ProductListModel>(_mapper.MapToAdminListModel(entries), pager);
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id, true);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.MapToFormModel(entity));
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductFormModel model)
        {
            var entity = new Product();

            _mapper.Map(model, entity);

            await _manager.CreateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductFormModel model)
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
