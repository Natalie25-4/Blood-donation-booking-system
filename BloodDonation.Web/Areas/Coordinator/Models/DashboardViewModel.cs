using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Areas.Coordinator.Models;

public class DashboardViewModel
{
    public double DeferralRatePercentage { get; set; }
    public double AverageBookingLeadTimeDays { get; set; }
    public IReadOnlyList<(AppointmentStatus Status, int Count)> AppointmentCountsByStatus { get; set; } = Array.Empty<(AppointmentStatus, int)>();
    public int TotalAppointments { get; set; }
}
