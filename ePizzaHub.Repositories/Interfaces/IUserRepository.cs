using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        UserModel ValidateUser(string Email, string Password);
        bool CreateUser(User user, string Role);
    }
}
