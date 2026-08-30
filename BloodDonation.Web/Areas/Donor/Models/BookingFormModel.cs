using System.ComponentModel.DataAnnotations;
using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Areas.Donor.Models;

public class BookingFormModel
{
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-30);

    [Required]
    [Range(1, 300)]
    [Display(Name = "Weight (kg)")]
    public double WeightKg { get; set; } = 70;

    [DataType(DataType.Date)]
    [Display(Name = "Last donation date (leave blank if first-time donor)")]
    public DateTime? LastDonationDate { get; set; }

    [Display(Name = "Had a recent tattoo, piercing, or illness?")]
    public bool HasStandDownEvent { get; set; }

    [Display(Name = "Reason")]
    public StandDownReason? StandDownReason { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Event date")]
    public DateTime? StandDownEventDate { get; set; }

    [Range(1, 3650)]
    [Display(Name = "Required stand-down duration (days)")]
    public int? StandDownDurationDays { get; set; }
}
