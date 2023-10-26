using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt; 

namespace ePizzaHub.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        AppDbContext context
        {
            get
            {
                return _db as AppDbContext;
            }
        }
        public UserRepository(AppDbContext _db): base(_db)
        {

        }
        public bool CreateUser(User user, string Role)
        {
            try
            {
                user.Password = BC.HashPassword(user.Password);
                Role role = context.Roles.Where(r => r.Name == Role).FirstOrDefault();
                user.Roles.Add(role);
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            User user = context.Users.Include(u => u.Roles).Where(u => u.Email == Email).FirstOrDefault();
            if (user != null)
            {
                bool isVerified = BC.Verify(Password, user.Password);
                if (isVerified)
                {
                    UserModel model = new UserModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles.Select(r => r.Name).ToArray()
                    };
                    return model;
                }
            }
            return null;
        }
    }
}
