using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Data;

public class SampleDataStore
{
    public List<Donor> Donors { get; } = new();
    public List<Appointment> Appointments { get; } = new();
}
