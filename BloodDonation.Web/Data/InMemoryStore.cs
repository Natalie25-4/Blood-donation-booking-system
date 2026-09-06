using BloodDonation.Web.Models;

namespace BloodDonation.Web.Data
{
    // Temporary in-memory data store — no real database yet.
    // Resets whenever the app restarts. Shared across all users/sessions,
    // which is fine for a prototype demo but not real multi-user behaviour.
    public static class InMemoryStore
    {
        public static List<DonationSession> Sessions { get; } = new()
        {
            new DonationSession { Id = "S1", SessionDate = DateTime.Today.AddDays(5), Time = "9:00 AM - 12:00 PM", Location = "Auckland City Centre", Capacity = 8, BookedCount = 2 },
            new DonationSession { Id = "S2", SessionDate = DateTime.Today.AddDays(7), Time = "1:00 PM - 4:00 PM", Location = "Auckland City Centre", Capacity = 6, BookedCount = 4 },
            new DonationSession { Id = "S3", SessionDate = DateTime.Today.AddDays(9), Time = "9:00 AM - 12:00 PM", Location = "North Shore Clinic", Capacity = 10, BookedCount = 1 },
            new DonationSession { Id = "S4", SessionDate = DateTime.Today.AddDays(10), Time = "10:00 AM - 2:00 PM", Location = "North Shore Clinic", Capacity = 5, BookedCount = 5 },
        };

        public static List<Booking> Bookings { get; } = new();

        // Seeded starting stock — represents existing inventory before the
        // system started tracking donations made through it.
        public static Dictionary<string, int> StockLevels { get; } = new()
        {
            { "O+", 14 }, { "O-", 6 }, { "A+", 11 }, { "A-", 4 },
            { "B+", 8 }, { "B-", 3 }, { "AB+", 5 }, { "AB-", 2 }
        };
    }
}