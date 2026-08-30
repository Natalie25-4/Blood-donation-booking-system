namespace BloodDonation.Domain.Models;

public class Appointment
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    public int SessionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ScheduledDate { get; set; }
    public AppointmentStatus Status { get; set; }
}
