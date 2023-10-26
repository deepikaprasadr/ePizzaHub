using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class CartService : ICartService
    {
        ICartRepository _cartRepo;
        IRepository<CartItem> _cartItem;
        public CartService(IRepository<CartItem> cartItem, ICartRepository cartRepo)
        {
            _cartItem = cartItem;
            _cartRepo = cartRepo;
        }
        public Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity)
        {
            try
            {
                Cart cart = _cartRepo.GetCart(CartId);
                if(cart == null)
                {
                    cart = new Cart();
                    CartItem item = new CartItem { ItemId = ItemId, Quantity = Quantity, UnitPrice = UnitPrice };
                    cart.Id = CartId;
                    cart.UserId = UserId;
                    cart.CreatedDate = DateTime.Now;
                    cart.IsActive = true;
                    item.CartId = cart.Id;
                    cart.CartItems.Add(item);
                    _cartRepo.Add(cart);
                    _cartRepo.SaveChanges();
                }
                else
                {
                    //if cart already exists
                    CartItem item = cart.CartItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity += Quantity;
                        _cartItem.Update(item);
                        _cartItem.SaveChanges();
                    }
                    else
                    {
                        item = new CartItem { ItemId = ItemId, Quantity = Quantity, UnitPrice = UnitPrice };
                        item.CartId = cart.Id;
                        cart.CartItems.Add(item);

                        _cartItem.Update(item);
                        _cartItem.SaveChanges();
                    }
                }
                return cart;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetCartCount(Guid cartId)
        {
            var cart = _cartRepo.GetCart(cartId);
            return cart != null ? cart.CartItems.Count() : 0;
        }
        public int DeleteItem(Guid cartId, int ItemId)
        {
            return _cartRepo.DeleteItem(cartId, ItemId);
        }

        public CartModel GetCartDetails(Guid cartId)
        {
            var model = _cartRepo.GetCartDetails(cartId);
            if (model != null && model.Items.Count > 0)
            {
                decimal subTotal = 0;
                foreach (var item in model.Items)
                {
                    item.Total = item.UnitPrice * item.Quantity;
                    subTotal += item.Total;
                }
                model.Total = subTotal;
                //5% tax
                model.Tax = Math.Round((model.Total * 5) / 100, 2);
                model.GrandTotal = model.Tax + model.Total;
            }
            return model;
        }

        public int UpdateQuantity(Guid CartId, int Id, int Quantity)
        {
            return _cartRepo.UpdateQuantity(CartId, Id, Quantity);
        }
        public int UpdateCart(Guid CartId, int UserId)
        {
            return _cartRepo.UpdateCart(CartId, UserId);
        }
    }
}
