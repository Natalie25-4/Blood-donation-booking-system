using BloodDonation.Web.Models;

namespace BloodDonation.Web.Data;

public static class SessionStats
{
    public static int CountOn(
        IEnumerable<DonationSession> sessions,
        DateTime date
    )
    {
        // test first implementation: return total
        //session count so new boundary test show the defect
        return sessions.Count();
    }
}