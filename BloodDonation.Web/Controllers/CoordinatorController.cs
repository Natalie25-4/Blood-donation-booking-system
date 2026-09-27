using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloodDonation.Web.Data;

namespace BloodDonation.Web.Controllers
{
    [Authorize(Roles = Roles.Coordinator)]
    public class CoordinatorController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard(string? bloodType, string? status)
        {
            var bookings = InMemoryStore.Bookings.AsEnumerable();

            if (!string.IsNullOrEmpty(bloodType))
                bookings = bookings.Where(b => b.BloodType == bloodType);

            if (!string.IsNullOrEmpty(status))
                bookings = bookings.Where(b => b.Status == status);

            var filteredBookings = bookings.ToList();

            var totalCapacity = InMemoryStore.Sessions.Sum(s => s.Capacity);
            var totalBooked = InMemoryStore.Sessions.Sum(s => s.BookedCount);
            var totalBookings = InMemoryStore.Bookings.Count;
            var deferredBookings = InMemoryStore.Bookings.Where(b => b.Status == "Deferred").ToList();
            var noShowCount = InMemoryStore.Bookings.Count(b => b.Status == "NoShow");

            // Average lead time: days between when the booking was made and the session date.
            var leadTimes = InMemoryStore.Bookings
                .Select(b => InMemoryStore.Sessions.FirstOrDefault(s => s.Id == b.SessionId))
                .Where(s => s != null)
                .Select(s => (s!.SessionDate.Date - InMemoryStore.Bookings.First(b => b.SessionId == s.Id).CreatedAt.Date).TotalDays)
                .ToList();

            var avgLeadTime = leadTimes.Count > 0 ? Math.Round(leadTimes.Average(), 1) : 0;

            var deferralReasons = deferredBookings
                .GroupBy(b => b.DeferralReason ?? "Not specified")
                .Select(g => new { Reason = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList();

            ViewBag.TotalCapacity = totalCapacity;
            ViewBag.TotalBooked = totalBooked;
            ViewBag.DeferralRate = totalBookings > 0 ? Math.Round((double)deferredBookings.Count / totalBookings * 100, 1) : 0;
            ViewBag.NoShowRate = totalBookings > 0 ? Math.Round((double)noShowCount / totalBookings * 100, 1) : 0;
            ViewBag.AvgLeadTime = avgLeadTime;
            ViewBag.DeferralReasons = deferralReasons;
            ViewBag.StockLevels = InMemoryStore.StockLevels;
            ViewBag.FilteredBookings = filteredBookings;
            ViewBag.SelectedBloodType = bloodType;
            ViewBag.SelectedStatus = status;

            return View();
        }
    }
}