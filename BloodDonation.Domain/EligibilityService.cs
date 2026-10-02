using BloodDonation.Domain.Models;

namespace BloodDonation.Domain;

public class EligibilityService
{
    // NZBS: returning donors can donate up to their 81st birthday.
    public const int ReturningDonorAgeLimit = 81;

    // Returns false if fewer than 84 days have passed between lastDonationDate and bookingDate
    public bool IsIntervalEligible(DateTime lastDonationDate, DateTime bookingDate)
    {
        var interval = bookingDate - lastDonationDate;
        return interval.TotalDays >= 84;
    }

    // New donors 16-71; returning donors from 16 until their 81st birthday (FR3)
    public bool IsAgeEligible(DateTime dateOfBirth, DateTime bookingDate, bool isNewDonor)
    {
        var age = bookingDate.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > bookingDate.AddYears(-age)) age--;

        if (isNewDonor)
        {
            return age >= 16 && age <= 71;
        }
        else
        {
            return age >= 16 && age < ReturningDonorAgeLimit;
        }
    }

    // 50kg minimum (FR4)
    public bool IsWeightEligible(double weightKg)
    {
        return weightKg >= 50;
    }

    // Stand-down rule (FR5)
    public bool IsStandDownCleared(StandDownEvent? standDownEvent, DateTime bookingDate)
    {
        if (standDownEvent == null)
        {
            return true; // No stand-down event, eligible
        }

        var standDownEndDate = standDownEvent.EventDate.AddDays(standDownEvent.DurationDays);
        return bookingDate >= standDownEndDate;
    }
}
