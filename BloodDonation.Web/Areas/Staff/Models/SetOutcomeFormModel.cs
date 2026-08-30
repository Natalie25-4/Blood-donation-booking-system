using System.ComponentModel.DataAnnotations;
using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Areas.Staff.Models;

public class SetOutcomeFormModel
{
    public int AppointmentId { get; set; }

    [Required]
    [Display(Name = "Outcome")]
    public AppointmentStatus? Outcome { get; set; }

    [StringLength(500)]
    [Display(Name = "Reason (required if deferred)")]
    public string? Reason { get; set; }
}
