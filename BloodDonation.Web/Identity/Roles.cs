using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Identity;

// Role names used by Identity and [Authorize]. Kept identical to the domain UserRole enum.
public static class Roles
{
    public const string Donor = nameof(UserRole.Donor);
    public const string Staff = nameof(UserRole.Staff);
    public const string Coordinator = nameof(UserRole.Coordinator);

    public static readonly string[] All = { Donor, Staff, Coordinator };
}
