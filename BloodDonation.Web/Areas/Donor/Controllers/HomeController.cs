using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Donor.Controllers;

[Area("Donor")]
[Authorize(Roles = Roles.Donor)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
