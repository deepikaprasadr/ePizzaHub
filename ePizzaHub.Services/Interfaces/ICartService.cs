using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Services.Interfaces
{
    public interface ICartService
    {
        int GetCartCount(Guid cartId);
        CartModel GetCartDetails(Guid cartId);
        Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity);
        int DeleteItem(Guid cartId, int ItemId);
        int UpdateQuantity(Guid CartId, int Id, int Quantity);
        int UpdateCart(Guid CartId, int UserId);
    }
}
