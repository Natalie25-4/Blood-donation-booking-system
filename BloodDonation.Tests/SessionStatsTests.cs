using BloodDonation.Web.Data;
using BloodDonation.Web.Models;

namespace BloodDonation.Tests;

[TestClass]

public class SessionStatTests
{
    
    [TestMethod]
    public void CountOn_OnlyCountsSessionsOnRequestedDate()
    {
        
        //fixed date to ensure test is repeatable
        var day = new DateTime(2026, 10, 3);
        
        var sessions = new[]
        {
            new DonationSession
            {
                Id = "S1",
                SessionDate = day.AddDays(-1)
            },

            new DonationSession{
                Id = "S2",
                SessionDate = day
            },

            new DonationSession
            {
                Id = "S3",
                SessionDate = day.AddDays(1)
            }
        };

        var result = SessionStats.CountOn (sessions, day);
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void CountOn_CountsMultipleSessionsOnSameDate()
    {
        
        // two sessions on the same day should be counted
        var day = new DateTime (2026, 10, 3);

        var sessions = new[]
        {
            new DonationSession
            {
                Id = "S1",
                SessionDate = day.AddHours(9)
            },

            new DonationSession
            {
                Id = "S2",
                SessionDate = day.AddHours(14)
            }
        };

        var result = SessionStats.CountOn(sessions, day);
        Assert.AreEqual(2, result);
    }

    [TestMethod]

    public void CountOn_WithNoSessions_ReturnsZero()
    {
        
        //empty session list should return 0
        var day = new DateTime(2026, 10, 3);
        var sessions = Array.Empty<DonationSession>();
        var result = SessionStats.CountOn(sessions, day);
        Assert.AreEqual(0, result);
    }
}