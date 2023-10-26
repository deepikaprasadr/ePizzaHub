using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        OrderModel GetOrderDetails(string id);
        IEnumerable<Order> GetUserOrders(int UserId);
    }
}
