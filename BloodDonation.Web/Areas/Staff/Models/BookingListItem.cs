using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Areas.Staff.Models;

public class BookingListItem
{
    public required Appointment Appointment { get; init; }
    public string? DeferralReason { get; init; }
}
