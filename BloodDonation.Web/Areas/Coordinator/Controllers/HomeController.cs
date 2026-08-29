using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Coordinator.Controllers;

[Area("Coordinator")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
