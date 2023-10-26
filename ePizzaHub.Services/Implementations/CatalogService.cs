using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{//register dependencies at app startup in project root level program.cs
    public class CatalogService : ICatalogService
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<ItemType> _itemTypeRepo;
        public CatalogService(IRepository<Item> itemRepo, IRepository<Category> categoryRepo, IRepository<ItemType> itemTypeRepo)
        {
            _itemRepo = itemRepo;
            _categoryRepo = categoryRepo;   
            _itemTypeRepo = itemTypeRepo;
        }

        public void AddItem(Item item)
        {
            _itemRepo.Add(item);
            _itemRepo.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            _itemRepo.Delete(id);
            _itemRepo.SaveChanges();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepo.GetAll();
        }

        //find it based on id
        public Item GetItem(int id)
        {
            return _itemRepo.Find(id);
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemRepo.GetAll();
        }

        public IEnumerable<ItemType> GetItemTypes()
        {
            return _itemTypeRepo.GetAll();
        }

        public void UpdateItem(Item item)
        {
            _itemRepo.Update(item);
            _itemRepo.SaveChanges();
        }
    }
}
