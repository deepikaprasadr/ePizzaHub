using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ePizzaHub.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        AppDbContext dbContext
        {
            get
            {
                return _db as AppDbContext;
            }
        }
        public OrderRepository(AppDbContext _db) : base(_db)
        {

        }
        public OrderModel GetOrderDetails(string id)
        {
            var model = (from order in dbContext.Orders
                         join payment in dbContext.PaymentDetails
                         on order.PaymentId equals payment.Id
                         where order.Id == id
                         select new OrderModel
                         {
                             Id = order.Id,
                             UserId = order.UserId,
                             CreatedDate = order.CreatedDate,
                             Items = (from orderItem in dbContext.OrderItems
                                      join item in dbContext.Items
                                      on orderItem.ItemId equals item.Id
                                      where orderItem.OrderId == id
                                      select new ItemModel
                                      {
                                          Id = orderItem.Id,
                                          Name = item.Name,
                                          Description = item.Description,
                                          ImageUrl = item.ImageUrl,
                                          Quantity = orderItem.Quantity,
                                          ItemId = item.Id,
                                          UnitPrice = orderItem.UnitPrice
                                      }).ToList(),
                             Total = payment.Total,
                             Tax = payment.Tax,
                             GrandTotal = payment.GrandTotal
                         }).FirstOrDefault();
            return model;
        }

        public IEnumerable<Order> GetUserOrders(int UserId)
        {
            return dbContext.Orders
                 .Include(o => o.OrderItems)
                 .Where(x => x.UserId == UserId).ToList();
        }
    }
}
