using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace RentACar.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
