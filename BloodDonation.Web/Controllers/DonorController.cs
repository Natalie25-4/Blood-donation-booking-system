using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloodDonation.Web.Data;
using BloodDonation.Web.Models;

namespace BloodDonation.Web.Controllers
{
    [Authorize(Roles = Roles.Donor)]
    public class DonorController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            var bookingId = HttpContext.Session.GetString("MyBookingId");
            Booking? booking = null;
            DonationSession? session = null;

            if (!string.IsNullOrEmpty(bookingId))
            {
                booking = InMemoryStore.Bookings.FirstOrDefault(b => b.Id == bookingId);
                if (booking != null)
                    session = InMemoryStore.Sessions.FirstOrDefault(s => s.Id == booking.SessionId);
            }

            ViewBag.Booking = booking;
            ViewBag.Session = session;
            return View();
        }

        [HttpGet]
        public IActionResult Book()
        {
            ViewBag.Sessions = InMemoryStore.Sessions.OrderBy(s => s.SessionDate).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Book(string SessionId, string DonorName, string DonorEmail, string BloodType, string? Notes)
        {
            var session = InMemoryStore.Sessions.FirstOrDefault(s => s.Id == SessionId);
            if (session == null || session.SpotsLeft <= 0)
            {
                TempData["Error"] = "That session is no longer available. Please choose another.";
                return RedirectToAction("Book");
            }

            var booking = new Booking
            {
                DonorName = DonorName,
                DonorEmail = DonorEmail,
                SessionId = SessionId,
                BloodType = BloodType,
                Notes = Notes
            };

            InMemoryStore.Bookings.Add(booking);
            session.BookedCount++;

            HttpContext.Session.SetString("MyBookingId", booking.Id);

            TempData["Message"] = $"Your appointment on {session.SessionDate:dddd d MMM yyyy} at {session.Location} is confirmed.";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Cancel(string BookingId)
        {
            var booking = InMemoryStore.Bookings.FirstOrDefault(b => b.Id == BookingId);
            if (booking != null)
            {
                var session = InMemoryStore.Sessions.FirstOrDefault(s => s.Id == booking.SessionId);
                if (session != null) session.BookedCount--;

                InMemoryStore.Bookings.Remove(booking);
                HttpContext.Session.Remove("MyBookingId");

                TempData["Message"] = "Your appointment has been cancelled.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Reschedule(string BookingId)
        {
            Cancel(BookingId);
            TempData["Message"] = "Your appointment was cancelled — choose a new session below.";
            return RedirectToAction("Book");
        }
    }
}