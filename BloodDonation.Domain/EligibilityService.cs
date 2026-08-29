using BloodDonation.Domain.Models;

namespace BloodDonation.Domain;

public class EligibilityService
{
    // 84-day rule (FR2)
    public bool IsIntervalEligible(DateTime lastDonationDate, DateTime bookingDate)
    {
        throw new NotImplementedException();
    }

    // 16-71 rule (FR3)
    public bool IsAgeEligible(DateTime dateOfBirth, DateTime bookingDate, bool isNewDonor)
    {
        throw new NotImplementedException();
    }

    // 50kg minimum (FR4)
    public bool IsWeightEligible(double weightKg)
    {
        throw new NotImplementedException();
    }

    // Stand-down rule (FR5)
    public bool IsStandDownCleared(StandDownEvent standDownEvent, DateTime bookingDate)
    {
        throw new NotImplementedException();
    }
}
