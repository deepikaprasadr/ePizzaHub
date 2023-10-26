using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class AuthService : IAuthService
    {
        IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CreateUser(User user, string Role)
        {
            return _userRepository.CreateUser(user, Role);
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            return _userRepository.ValidateUser(Email, Password);
        }
    }
}
