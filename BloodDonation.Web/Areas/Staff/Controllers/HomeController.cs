using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Staff.Controllers;

[Area("Staff")]
[Authorize(Roles = Roles.Staff)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
