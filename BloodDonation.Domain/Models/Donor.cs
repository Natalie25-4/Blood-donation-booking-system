namespace BloodDonation.Domain.Models;

public class Donor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public double WeightKg { get; set; }
    public DateTime? LastDonationDate { get; set; }
    public UserRole Role { get; set; }
}
