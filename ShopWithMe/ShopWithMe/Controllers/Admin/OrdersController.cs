using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Orders;
using ShopWithMe.Managers.Products;
using ShopWithMe.Models.Admin;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Cart.FormModels;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Orders.Admin;
using ShopWithMe.Tools.Exceptions;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    public class OrdersController : ControllerBase
    {
        protected IOrderRepository _repository;
        protected OrdersManager _manager;
        protected ProductsManager _productsManager;
        protected OrderModelsMapper _mapper;
        protected Cart _cart;
        protected ContactData _contactData;

        #region OrdersController()
        public OrdersController(
            IOrderRepository repository,
            OrdersManager manager,
            ProductsManager productsManager,
            OrderModelsMapper mapper,
            Cart cart,
            ContactData contactData)
        {
            _repository = repository;
            _manager = manager;
            _productsManager = productsManager;
            _mapper = mapper;
            _cart = cart;
            _contactData = contactData;
        }
        #endregion

        #region GetList()
        [HttpGet]
        [Authorize]
        public async Task<ResultModel<OrderListModel>> GetList([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var pager = new Pager()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var entries = await _manager.GetListAsync(pager);

            return new ResultModel<OrderListModel>(_mapper.MapToAdminListModel(entries), pager);
        }
        #endregion

        #region Get()
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var entity = await _manager.GetAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.MapToAdminDetailsModel(entity));
        }
        #endregion

        #region Update()
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderFormModel model)
        {
            var entity = await _manager.GetAsync(model.Id);

            if (entity == null)
                return NotFound();

            _mapper.Map(model, entity);

            await _manager.UpdateAsync(entity);

            return Ok(entity);
        }
        #endregion

        #region UpdateCartLine()
        [HttpPost("cart-line")]
        public async Task<IActionResult> UpdateCartLine([FromBody] OrderCartLineUpdateModel model)
        {
            try
            {
                var product = await _productsManager.GetAsync(model.ProductId);

                if (product == null)
                    return NotFound();

                return Ok(await _manager.UpdateCartLine(model, product));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        #endregion

        #region RemoveCartLine()
        [HttpDelete("cart-line")]
        public async Task<IActionResult> RemoveCartLine([FromBody] OrderCartLineUpdateModel model)
        {
            try
            {
                return Ok(await _manager.RemoveCartLine(model));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
