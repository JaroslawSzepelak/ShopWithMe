﻿using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Cart.FormModels;
using ShopWithMe.Models.Common;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Exceptions;

namespace ShopWithMe.Managers.Orders
{
    public class OrdersManager : BaseManager<Order>
    {
        #region OrdersManager()
        public OrdersManager(IBaseRepository<Order> repository) : base(repository) { }
        #endregion

        #region GetAsync()
        public override async Task<Order> GetAsync(long id)
        {
            return await _repository.Entities
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
        #endregion

        #region UpdateCartLine()
        public async Task<List<CartLine>> UpdateCartLine(OrderCartLineUpdateModel model, Product product)
        {
            var order = await _repository.Entities
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null)
            {
                throw new NotFoundException();
            }

            var cartLine = order.Lines.FirstOrDefault(c => c.Product.Id == model.ProductId);

            if (cartLine != null)
            {
                if (model.Quantity.HasValue)
                {
                    cartLine.Quantity = model.Quantity.Value;
                }
                else
                {
                    cartLine.Quantity++;
                }
            }
            else
            {
                cartLine = new CartLine()
                {
                    Product = product,
                    Quantity = model.Quantity ?? 1
                };

                order.Lines.Add(cartLine);
            }

            _repository.SaveChanges();

            return order.Lines.ToList();
        }
        #endregion

        #region RemoveCartLine()
        public async Task<List<CartLine>> RemoveCartLine(OrderCartLineUpdateModel model)
        {
            var order = await _repository.Entities
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null)
            {
                throw new NotFoundException();
            }

            var cartLine = order.Lines.FirstOrDefault(c => c.Product.Id == model.ProductId);

            if (cartLine != null)
            {
                order.Lines.Remove(cartLine);

                _repository.SaveChanges();
            }

            return order.Lines.ToList();
        }
        #endregion
    }
}