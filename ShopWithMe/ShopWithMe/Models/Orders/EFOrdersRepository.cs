using Microsoft.EntityFrameworkCore;
using ShopWithMe.Tools.Exceptions;

namespace ShopWithMe.Models.Orders
{
    public class EFOrderRepository : IOrderRepository
    {
        private DefaultContext _context;
        public IQueryable<Order> Orders => _context.Orders;

        #region EFOrderRepository()
        public EFOrderRepository(DefaultContext context)
        {
            _context = context;
        }
        #endregion

        #region GetList()
        public async Task<IEnumerable<Order>> GetList()
        {
            return await _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .ToListAsync();
        }
        #endregion

        #region SaveOrder
        public async Task SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));

            if (order.Id == 0)
            {
                await _context.Orders.AddAsync(order);
            }

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete()
        public async Task Delete(long id)
        {
            var entity = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
