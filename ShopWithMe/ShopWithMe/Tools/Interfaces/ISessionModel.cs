namespace ShopWithMe.Tools.Interfaces
{
    public interface ISessionModel
    {
        public static string SessionKey { get; }
        public ISession Session { get; set; }
    }
}
