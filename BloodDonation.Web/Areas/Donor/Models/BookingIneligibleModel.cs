namespace BloodDonation.Web.Areas.Donor.Models;

public class BookingIneligibleModel
{
    public string FailedRule { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime? EarliestEligibleDate { get; set; }
}
