namespace BloodDonation.Domain.Models;

public class StandDownEvent
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    public StandDownReason Reason { get; set; }
    public DateTime EventDate { get; set; }
    public int DurationDays { get; set; }
}
