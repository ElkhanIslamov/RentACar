using Microsoft.AspNetCore.Mvc;

namespace RentACar.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
