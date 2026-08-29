using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Donor.Controllers;

[Area("Donor")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
