namespace BloodDonation.Web.Models
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? DonorUserId { get; set; } // Identity user who made the booking
        public string DonorName { get; set; } = string.Empty;
        public string DonorEmail { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string Status { get; set; } = "Confirmed"; // Confirmed, Deferred, Donated, NoShow
        public string? DeferralReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}