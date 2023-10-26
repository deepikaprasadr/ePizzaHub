using ePizzaHub.Core.Entities;


namespace ePizzaHub.Services.Interfaces
{
    public interface ICatalogService
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<ItemType> GetItemTypes();
        IEnumerable<Item> GetItems();
        Item GetItem(int id);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int id);
    }
}
