using ePizzaHub.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ePizzaHub.UI.Helpers
{
    public class UserAccessor : IUserAccessor
    {
        IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserModel GetUser()
        {
            if (_httpContextAccessor.HttpContext.User != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string userData = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                UserModel user = JsonSerializer.Deserialize<UserModel>(userData);
                return user;
            }
            return null;
        }
    }
}
