using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Areas.User.Controllers
{  

    public class HomeController : BaseController
    {
   
        public IActionResult Index()
        {
            return View();
        }
    }
}
