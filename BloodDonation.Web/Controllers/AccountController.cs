using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            string FullName,
            string Email,
            string Password,
            string ConfirmPassword,
            string DateOfBirth,
            decimal WeightKg,
            string? LastDonationDate,
            string? TattooOrPiercingDate,
            bool RecentIllness,
            bool RecentCovid,
            bool Pregnant,
            string? TravelledOverseas,
            bool RecentBloodTransfusion,
            bool RecentDentalWork)
        {
            // Placeholder: no real account creation yet, just redirect through
            return RedirectToAction("Dashboard", "Donor");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            return RedirectToAction("Dashboard", "Donor");
        }

        [HttpGet]
        public IActionResult StaffLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StaffLogin(string Email, string Password)
        {
            return RedirectToAction("Dashboard", "Staff");
        }
    }
}