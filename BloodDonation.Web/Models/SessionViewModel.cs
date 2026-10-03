using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Web.Models
{
    public class SessionViewModel
    {
        // The session date is required and should be entered as a date.
        [Required]
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }

        // The session time is required.
        [Required]
        public string Time { get; set; } = string.Empty;

        // The session location is required.
        [Required]
        public string Location { get; set; } = string.Empty;

        // Capacity is required and must be at least one person.
        [Required]
        [Range(1, 300)]
        public int Capacity { get; set; }
    }
}