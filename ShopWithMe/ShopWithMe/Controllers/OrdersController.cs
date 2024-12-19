﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Orders;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.ContactData;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Orders.Public;
using ShopWithMe.Tools.Models;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        protected IOrderRepository _repository;
        protected OrdersManager _manager;
        protected OrderModelsMapper _mapper;
        protected Cart _cart;
        protected ContactData _contactData;
        protected UserManager<IdentityUser> _userManager;

        #region OrdersController()
        public OrdersController(
            IOrderRepository repository,
            OrdersManager manager,
            OrderModelsMapper mapper,
            Cart cart,
            ContactData contactData,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _manager = manager;
            _mapper = mapper;
            _cart = cart;
            _contactData = contactData;
            _userManager = userManager;
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

            return new ResultModel<OrderListModel>(_mapper.MapToPublicListModel(entries), pager);
        }
        #endregion

        #region GetSummary()
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            return Ok(new OrderSummaryModel(_cart, _contactData));
        }
        #endregion

        #region Create()
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var entity = new Order(_cart, _contactData);
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
                entity.UserId = user.Id;

            await _repository.SaveOrder(entity);

            _cart.Clear();
            _contactData.ClearContactData();

            return Ok(entity);
        }
        #endregion
    }
}
