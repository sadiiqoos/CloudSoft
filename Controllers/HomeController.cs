using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using CloudSoft.Models;

namespace CloudSoft.Controllers;
=======
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;
>>>>>>> 452c3f33b7908495a5fe842ab412cadfeedc0234

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
