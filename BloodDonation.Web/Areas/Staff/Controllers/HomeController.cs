using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Staff.Controllers;

[Area("Staff")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
