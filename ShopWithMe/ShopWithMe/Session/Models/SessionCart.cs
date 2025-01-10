using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Products;
using ShopWithMe.Tools.Abstractions;
using ShopWithMe.Utils.Extensions;
using System.Text.Json.Serialization;

namespace SportsStore.Models
{
    public class SessionCart : Cart, ISessionModel
    {
        public static string SessionKey => "Cart";

        [JsonIgnore]
        public ISession Session { get; set; }

        #region GetCart()
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>(SessionKey) ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        #endregion

        #region AddItem()
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson(SessionKey, this);
        }
        #endregion

        #region UpdateItem()
        public override void UpdateItem(CartLine line, int? quantity)
        {
            base.UpdateItem(line, quantity);
            Session.SetJson(SessionKey, this);
        }
        #endregion

        #region RemoveLine()
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson(SessionKey, this);
        }
        #endregion

        #region Clear()
        public override void Clear()
        {
            base.Clear();
            Session.Remove(SessionKey);
        }
        #endregion
    }
}
