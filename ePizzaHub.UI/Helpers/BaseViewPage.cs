using ePizzaHub.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ePizzaHub.UI.Helpers
{
    public abstract class BaseViewPage<TModel> : RazorPage<TModel>
    {
        [RazorInject]
        public IUserAccessor _userAccessor { get; set; }

        public UserModel CurrentUser
        {
            get
            {
                return _userAccessor.GetUser();
            }
        }
    }
}
