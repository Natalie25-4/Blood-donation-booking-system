using BloodDonation.Domain.Models;

namespace BloodDonation.Domain;

public sealed record AppointmentAnalytics(
    double DeferralRatePercentage,
    double AverageBookingLeadTimeDays);

public class AppointmentAnalyticsService
{
    public AppointmentAnalytics Calculate(IEnumerable<Appointment> appointments)
    {
        ArgumentNullException.ThrowIfNull(appointments);

        var appointmentList = appointments.ToList();
        var completedAppointments = appointmentList
            .Where(appointment => appointment.Status is
                AppointmentStatus.Donated or
                AppointmentStatus.Deferred or
                AppointmentStatus.NoShow)
            .ToList();

        var deferralRatePercentage = completedAppointments.Count == 0
            ? 0
            : completedAppointments.Count(appointment => appointment.Status == AppointmentStatus.Deferred)
                * 100d / completedAppointments.Count;

        var averageBookingLeadTimeDays = appointmentList.Count == 0
            ? 0
            : appointmentList.Average(appointment =>
                (appointment.ScheduledDate - appointment.CreatedDate).TotalDays);

        return new AppointmentAnalytics(
            deferralRatePercentage,
            averageBookingLeadTimeDays);
    }
}
