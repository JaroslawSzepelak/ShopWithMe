namespace ShopWithMe.Models.Orders
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        IAsyncEnumerable<Order> GetList();
        Task SaveOrder(Order order);
        Task Delete(long id);
    }
}
