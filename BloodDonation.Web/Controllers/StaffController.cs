using Microsoft.AspNetCore.Mvc;
using BloodDonation.Web.Data;

namespace BloodDonation.Web.Controllers
{
    public class StaffController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.TodaySessions = InMemoryStore.Sessions.Count;
            ViewBag.TotalBookings = InMemoryStore.Bookings.Count;
            ViewBag.FlaggedCount = InMemoryStore.Bookings.Count(b => b.Status == "Deferred");
            ViewBag.NoShowCount = InMemoryStore.Bookings.Count(b => b.Status == "NoShow");
            return View();
        }

        [HttpGet]
        public IActionResult Bookings()
        {
            var bookings = InMemoryStore.Bookings
                .Select(b => new
                {
                    Booking = b,
                    Session = InMemoryStore.Sessions.FirstOrDefault(s => s.Id == b.SessionId)
                })
                .ToList();

            ViewBag.Bookings = bookings;
            return View();
        }

        [HttpPost]
        public IActionResult RecordOutcome(string BookingId, string Outcome, string? DeferralReason)
        {
            var booking = InMemoryStore.Bookings.FirstOrDefault(b => b.Id == BookingId);
            if (booking != null)
            {
                booking.Status = Outcome;

                if (Outcome == "Deferred")
                {
                    booking.DeferralReason = string.IsNullOrEmpty(DeferralReason) ? "Not specified" : DeferralReason;
                }

                if (Outcome == "Donated" && !string.IsNullOrEmpty(booking.BloodType)
                    && InMemoryStore.StockLevels.ContainsKey(booking.BloodType))
                {
                    InMemoryStore.StockLevels[booking.BloodType]++;
                }
            }
            return RedirectToAction("Bookings");
        }

        [HttpGet]
        public IActionResult Sessions()
        {
            ViewBag.Sessions = InMemoryStore.Sessions.OrderBy(s => s.SessionDate).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Sessions(DateTime SessionDate, string Time, string Location, int Capacity)
        {
            InMemoryStore.Sessions.Add(new Models.DonationSession
            {
                Id = "S" + (InMemoryStore.Sessions.Count + 1),
                SessionDate = SessionDate,
                Time = Time,
                Location = Location,
                Capacity = Capacity,
                BookedCount = 0
            });

            TempData["Message"] = "New session added.";
            return RedirectToAction("Sessions");
        }
    }
}