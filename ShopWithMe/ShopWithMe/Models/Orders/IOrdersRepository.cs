namespace ShopWithMe.Models.Orders
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        Task<IEnumerable<Order>> GetList();
        Task SaveOrder(Order order);
        Task Delete(long id);
    }
}
