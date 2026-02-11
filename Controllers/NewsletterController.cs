using CloudSoft.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudSoft.Controllers;

public class NewsletterController : Controller
{

    // GET: /Newsletter/Subscribe
    public IActionResult Subscribe()
    {
        return View();
    }

    // POST: /Newsletter/Subscribe
    [HttpPost]
    public IActionResult Subscribe(Subscriber subscriber)
    {
        // Add subscription logic here
        // ...

        // Write to the console
        string value = $"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}";
        Console.WriteLine(value: value);

        // Send a message to the user
        return Content($"Thank you {subscriber.Name} for subscribing to our newsletter!");
    }
}
