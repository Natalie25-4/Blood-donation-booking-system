using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Controllers
{
    public class StaffController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Bookings()
        {
            return View();
        }
    }
}