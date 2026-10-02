namespace BloodDonation.Domain;

// NZBS eligibility thresholds:
// https://www.nzblood.co.nz/become-a-donor/am-i-eligible/detailed-eligibility-criteria
public static class EligibilityRules
{
    public const int MinimumDonationIntervalDays = 84;
    public const double MinimumWeightKg = 50;
    public const int MinimumAge = 16;
    public const int NewDonorMaximumAge = 71;

    // Returning donors can donate up to their 81st birthday.
    public const int ReturningDonorAgeLimit = 81;
}
