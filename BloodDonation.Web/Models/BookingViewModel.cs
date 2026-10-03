using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Web.Models
{
    public class BookingViewModel
    {
        //session must be selected when creating a booking
        [Required]
        public string SessionId { get; set; } = string.Empty;

        //donor name is required for the booking
        [Required]
        public string DonorName { get; set; } = string.Empty;

        //donor email is required and must be a valid email address
        [Required]
        [EmailAddress]
        public string DonorEmail { get; set; } = string.Empty;

        //blood type is optional when creating a booking
        public string? BloodType { get; set; }

        //notes are optional when creating a booking
        public string? Notes { get; set; }
    }
}