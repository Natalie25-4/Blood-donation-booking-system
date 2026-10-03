using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Web.Models
{
    public class RegistrationViewModel
    {
        //full name is required when creating a donor account
        [Required]
        public string FullName { get; set; } = string.Empty;

        //email is required and must be a valid email address
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        //password is required for the donor account
        [Required]
        public string Password { get; set; } = string.Empty;

        //confirm password must match the password entered above
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        //date of birth is required for the donor questionnaire
        [Required]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; } = string.Empty;

        //weight must be entered during registration
        [Required]
        [Range(0.1, 200)]
        public decimal WeightKg { get; set; }

        //optional questionnaire dates
        [DataType(DataType.Date)]
        public string? LastDonationDate { get; set; }

        [DataType(DataType.Date)]
        public string? TattooOrPiercingDate { get; set; }

        //eligibility questionnaire answers
        public bool RecentIllness { get; set; }

        public bool RecentCovid { get; set; }

        public bool Pregnant { get; set; }

        [Required]
        public string? TravelledOverseas { get; set; }

        public bool RecentBloodTransfusion { get; set; }

        public bool RecentDentalWork { get; set; }
    }
}