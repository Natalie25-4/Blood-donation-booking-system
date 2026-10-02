using BloodDonation.Domain.Models;
using static BloodDonation.Domain.EligibilityRules;

namespace BloodDonation.Domain;

public class EligibilityService
{
    // Returns false if fewer than 84 days have passed between lastDonationDate and bookingDate.
    // A first-time donor (no last donation) has no interval to wait (TC09).
    public bool IsIntervalEligible(DateTime? lastDonationDate, DateTime bookingDate)
    {
        if (lastDonationDate is null)
        {
            return true;
        }

        var interval = bookingDate - lastDonationDate.Value;
        return interval.TotalDays >= MinimumDonationIntervalDays;
    }

    // New donors 16-71; returning donors from 16 until their 81st birthday (FR3)
    public bool IsAgeEligible(DateTime dateOfBirth, DateTime bookingDate, bool isNewDonor)
    {
        var age = CalculateAge(dateOfBirth, bookingDate);

        if (isNewDonor)
        {
            return age >= MinimumAge && age <= NewDonorMaximumAge;
        }
        else
        {
            return age >= MinimumAge && age < ReturningDonorAgeLimit;
        }
    }

    // 50kg minimum (FR4)
    public bool IsWeightEligible(double weightKg)
    {
        return weightKg >= MinimumWeightKg;
    }

    // Stand-down rule (FR5)
    public bool IsStandDownCleared(StandDownEvent? standDownEvent, DateTime bookingDate)
    {
        if (standDownEvent == null)
        {
            return true; // No stand-down event, eligible
        }

        return bookingDate >= StandDownEndDate(standDownEvent);
    }

    // Age in whole years on the given date.
    public int CalculateAge(DateTime dateOfBirth, DateTime onDate)
    {
        var age = onDate.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > onDate.AddYears(-age)) age--;
        return age;
    }

    // Earliest eligible dates for the time-based rules (FR3).
    public DateTime EarliestDonationDate(DateTime lastDonationDate)
    {
        return lastDonationDate.AddDays(MinimumDonationIntervalDays);
    }

    public DateTime MinimumAgeDate(DateTime dateOfBirth)
    {
        return dateOfBirth.AddYears(MinimumAge);
    }

    public DateTime StandDownEndDate(StandDownEvent standDownEvent)
    {
        return standDownEvent.EventDate.AddDays(standDownEvent.DurationDays);
    }
}
