using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Controllers
{
    public class DonorController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Book(string SessionId, string BloodType, string? Notes)
        {
            // Placeholder: no real booking persistence yet, just confirm and redirect
            TempData["BookingConfirmed"] = true;
            return RedirectToAction("Dashboard", "Donor");
        }
    }
}