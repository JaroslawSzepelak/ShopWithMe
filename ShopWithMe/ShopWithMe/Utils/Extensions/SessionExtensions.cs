using System.Text.Json;

namespace ShopWithMe.Utils.Extensions
{
    public static class SessionExtensions
    {
        #region GetJson()
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);

            return sessionData == null ? default : JsonSerializer.Deserialize<T>(sessionData);
        }
        #endregion

        #region SetJson()
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        #endregion
    }
}
