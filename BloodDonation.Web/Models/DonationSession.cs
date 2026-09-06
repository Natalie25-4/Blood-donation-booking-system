namespace BloodDonation.Web.Models
{
    public class DonationSession
    {
        public string Id { get; set; } = string.Empty;
        public DateTime SessionDate { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int BookedCount { get; set; }
        public int SpotsLeft => Capacity - BookedCount;
    }
}