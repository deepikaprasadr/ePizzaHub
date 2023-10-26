using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Interfaces
{
    //List<User>
    //IRepository<User>
   public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Find(object Id); //primary key -obj type generic
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(object Id);
        int SaveChanges();
    }

    public interface IRepository1<TEntity1> where TEntity1 : class
    {
        
    }
}
