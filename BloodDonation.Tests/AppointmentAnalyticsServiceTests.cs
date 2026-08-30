using BloodDonation.Domain;
using BloodDonation.Domain.Models;

namespace BloodDonation.Tests;

[TestClass]
public class AppointmentAnalyticsServiceTests
{
    private readonly AppointmentAnalyticsService _service = new();

    [TestMethod]
    public void Calculate_ExcludesBookedAndCancelledFromDeferralRate()
    {
        var appointments = new[]
        {
            new Appointment { Status = AppointmentStatus.Deferred },
            new Appointment { Status = AppointmentStatus.Donated },
            new Appointment { Status = AppointmentStatus.NoShow },
            new Appointment { Status = AppointmentStatus.Booked },
            new Appointment { Status = AppointmentStatus.Cancelled }
        };

        var result = _service.Calculate(appointments);

        Assert.AreEqual(33.333333333333336, result.DeferralRatePercentage, 0.000000001);
    }

    [TestMethod]
    public void Calculate_AveragesBookingLeadTimeAcrossAllAppointments()
    {
        var appointments = new[]
        {
            new Appointment
            {
                CreatedDate = new DateTime(2024, 1, 1),
                ScheduledDate = new DateTime(2024, 1, 11)
            },
            new Appointment
            {
                CreatedDate = new DateTime(2024, 2, 1),
                ScheduledDate = new DateTime(2024, 2, 21)
            }
        };

        var result = _service.Calculate(appointments);

        Assert.AreEqual(15, result.AverageBookingLeadTimeDays);
    }

    [TestMethod]
    public void Calculate_WithOnlyBookedAndCancelledAppointments_ReturnsZeroDeferralRateAndIncludesLeadTimes()
    {
        var appointments = new[]
        {
            new Appointment
            {
                Status = AppointmentStatus.Booked,
                CreatedDate = new DateTime(2024, 1, 1),
                ScheduledDate = new DateTime(2024, 1, 11)
            },
            new Appointment
            {
                Status = AppointmentStatus.Cancelled,
                CreatedDate = new DateTime(2024, 2, 1),
                ScheduledDate = new DateTime(2024, 2, 21)
            }
        };

        var result = _service.Calculate(appointments);

        Assert.AreEqual(0, result.DeferralRatePercentage);
        Assert.AreEqual(15, result.AverageBookingLeadTimeDays);
    }

    [TestMethod]
    public void Calculate_WithNoAppointments_ReturnsZeroMetrics()
    {
        var result = _service.Calculate(Array.Empty<Appointment>());

        Assert.AreEqual(0, result.DeferralRatePercentage);
        Assert.AreEqual(0, result.AverageBookingLeadTimeDays);
    }
}
