using ShopWithMe.Models.ContactData;
using ShopWithMe.Tools.Interfaces;
using ShopWithMe.Utils.Extensions;
using System.Text.Json.Serialization;

namespace ShopWithMe.Session.Models
{
    public class SessionContactData : ContactData, ISessionModel
    {
        public static string SessionKey => "ContactData";

        [JsonIgnore]
        public ISession Session { get; set; }

        #region GetContactData()
        public static ContactData GetContactData(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var cart = session?.GetJson<SessionContactData>(SessionKey) ?? new SessionContactData();
            cart.Session = session;
            return cart;
        }
        #endregion

        #region UpdateContactData()
        public override void UpdateContactData(ContactDataFormModel formModel)
        {
            base.UpdateContactData(formModel);
            Session.SetJson(SessionKey, this);
        }
        #endregion

        #region ClearContactData()
        public override void ClearContactData()
        {
            base.ClearContactData();
            Session.SetJson(SessionKey, this);
        }
        #endregion
    }
}
