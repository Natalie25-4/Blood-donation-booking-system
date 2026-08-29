namespace BloodDonation.Domain.Models;

public class Session
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int CapacitySlots { get; set; }
    public int BookedSlots { get; set; }
}
