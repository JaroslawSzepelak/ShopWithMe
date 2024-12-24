namespace ShopWithMe.Tools.Abstractions
{
    public interface ISessionModel
    {
        public static string SessionKey { get; }
        public ISession Session { get; set; }
    }
}
