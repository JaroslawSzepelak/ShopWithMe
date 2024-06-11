using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Products;
using ShopWithMe.Utils.Extensions;
using System;
using System.Text.Json.Serialization;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession Session { get; set; }

        #region GetCart()
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        #endregion

        #region AddItem()
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }
        #endregion

        #region RemoveLine()
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }
        #endregion

        #region Clear()
        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
        #endregion
    }
}
