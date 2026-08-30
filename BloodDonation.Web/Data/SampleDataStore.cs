using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Data;

public class SampleDataStore
{
    public List<Donor> Donors { get; } = new();
    public List<Appointment> Appointments { get; } = new();

    // Keyed by Appointment.Id. Only populated when an appointment's outcome is set to Deferred.
    public Dictionary<int, string> DeferralReasons { get; } = new();
}
