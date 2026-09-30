using BloodDonation.Domain;
using BloodDonation.Domain.Models;

namespace BloodDonation.Tests;

[TestClass]
public class EligibilityServiceTests
{
    private readonly EligibilityService _service = new();

    #region IsIntervalEligible Tests

    [TestMethod]
    public void IsIntervalEligible_83DaysInterval_ReturnsFalse()
    {
        // Arrange
        var lastDonationDate = new DateTime(2024, 1, 1);
        var bookingDate = lastDonationDate.AddDays(83);

        // Act
        var result = _service.IsIntervalEligible(lastDonationDate, bookingDate);

        // Assert
        Assert.IsFalse(result, "83 days should be ineligible (below 84-day minimum)");
    }

    [TestMethod]
    public void IsIntervalEligible_84DaysInterval_ReturnsTrue()
    {
        // Arrange
        var lastDonationDate = new DateTime(2024, 1, 1);
        var bookingDate = lastDonationDate.AddDays(84);

        // Act
        var result = _service.IsIntervalEligible(lastDonationDate, bookingDate);

        // Assert
        Assert.IsTrue(result, "84 days should be eligible (exactly meets 84-day minimum)");
    }

    [TestMethod]
    public void IsIntervalEligible_85DaysInterval_ReturnsTrue()
    {
        // Arrange
        var lastDonationDate = new DateTime(2024, 1, 1);
        var bookingDate = lastDonationDate.AddDays(85);

        // Act
        var result = _service.IsIntervalEligible(lastDonationDate, bookingDate);

        // Assert
        Assert.IsTrue(result, "85 days should be eligible (above 84-day minimum)");
    }

    #endregion

    #region IsAgeEligible Tests - New Donors

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age15_ReturnsFalse()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(2009, 6, 15); // Will be exactly 15 on booking date

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsFalse(result, "Age 15 should be ineligible for new donors (below 16-year minimum)");
    }

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age16_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(2008, 6, 15); // Will be exactly 16

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsTrue(result, "Age 16 should be eligible for new donors (exactly meets 16-year minimum)");
    }

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age17_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(2007, 6, 15); // Will be 17

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsTrue(result, "Age 17 should be eligible for new donors");
    }

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age70_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1954, 6, 15); // Will be 70

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsTrue(result, "Age 70 should be eligible for new donors");
    }

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age71_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1953, 6, 15); // Will be exactly 71

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsTrue(result, "Age 71 should be eligible for new donors (exactly meets 71-year maximum)");
    }

    [TestMethod]
    public void IsAgeEligible_NewDonor_Age72_ReturnsFalse()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1952, 6, 15); // Will be 72

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: true);

        // Assert
        Assert.IsFalse(result, "Age 72 should be ineligible for new donors (above 71-year maximum)");
    }

    #endregion

    #region IsAgeEligible Tests - Existing Donors

    [TestMethod]
    public void IsAgeEligible_ExistingDonor_Age15_ReturnsFalse()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(2009, 6, 15); // Exactly 15 on booking date

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: false);

        // Assert
        Assert.IsFalse(result, "Age 15 should be ineligible for existing donors (below 16-year minimum)");
    }

    [TestMethod]
    public void IsAgeEligible_ExistingDonor_Age16_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(2008, 6, 15); // Age 16

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: false);

        // Assert
        Assert.IsTrue(result, "Age 16 should be eligible for existing donors (exactly meets 16-year minimum)");
    }

    [TestMethod]
    public void IsAgeEligible_ExistingDonor_Age71_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1953, 6, 15); // Age 71

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: false);

        // Assert
        Assert.IsTrue(result, "Age 71 should be eligible for existing donors (no upper age limit)");
    }

    [TestMethod]
    public void IsAgeEligible_ExistingDonor_Age72_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1952, 6, 15); // Age 72

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: false);

        // Assert
        Assert.IsTrue(result, "Age 72 should be eligible for existing donors (no upper age limit)");
    }

    [TestMethod]
    public void IsAgeEligible_ExistingDonor_Age100_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);
        var dateOfBirth = new DateTime(1924, 6, 15); // Age 100

        // Act
        var result = _service.IsAgeEligible(dateOfBirth, bookingDate, isNewDonor: false);

        // Assert
        Assert.IsTrue(result, "Age 100 should be eligible for existing donors (no upper age limit)");
    }

    #endregion

    #region IsWeightEligible Tests

    [TestMethod]
    public void IsWeightEligible_49Kg_ReturnsFalse()
    {
        // Arrange
        var weight = 49.0;

        // Act
        var result = _service.IsWeightEligible(weight);

        // Assert
        Assert.IsFalse(result, "49 kg should be ineligible (below 50 kg minimum)");
    }

    [TestMethod]
    public void IsWeightEligible_50Kg_ReturnsTrue()
    {
        // Arrange
        var weight = 50.0;

        // Act
        var result = _service.IsWeightEligible(weight);

        // Assert
        Assert.IsTrue(result, "50 kg should be eligible (exactly meets 50 kg minimum)");
    }

    [TestMethod]
    public void IsWeightEligible_51Kg_ReturnsTrue()
    {
        // Arrange
        var weight = 51.0;

        // Act
        var result = _service.IsWeightEligible(weight);

        // Assert
        Assert.IsTrue(result, "51 kg should be eligible (above 50 kg minimum)");
    }

    [TestMethod]
    public void IsWeightEligible_49_9Kg_ReturnsFalse()
    {
        // Arrange
        var weight = 49.9;

        // Act
        var result = _service.IsWeightEligible(weight);

        // Assert
        Assert.IsFalse(result, "49.9 kg should be ineligible (below 50 kg minimum)");
    }

    [TestMethod]
    public void IsWeightEligible_50_1Kg_ReturnsTrue()
    {
        // Arrange
        var weight = 50.1;

        // Act
        var result = _service.IsWeightEligible(weight);

        // Assert
        Assert.IsTrue(result, "50.1 kg should be eligible (above 50 kg minimum)");
    }

    #endregion

    #region IsStandDownCleared Tests

    [TestMethod]
    public void IsStandDownCleared_NoStandDownEvent_ReturnsTrue()
    {
        // Arrange
        var bookingDate = new DateTime(2024, 6, 15);

        // Act
        var result = _service.IsStandDownCleared(null, bookingDate);

        // Assert
        Assert.IsTrue(result, "No stand-down event should be eligible");
    }

    [TestMethod]
    public void IsStandDownCleared_OneDayBeforeStandDownEnds_ReturnsFalse()
    {
        // Arrange
        var eventDate = new DateTime(2024, 1, 1);
        var durationDays = 14;
        var standDownEvent = new StandDownEvent
        {
            EventDate = eventDate,
            DurationDays = durationDays
        };
        var bookingDate = eventDate.AddDays(durationDays - 1); // One day before stand-down ends

        // Act
        var result = _service.IsStandDownCleared(standDownEvent, bookingDate);

        // Assert
        Assert.IsFalse(result, "Booking before stand-down ends should be ineligible");
    }

    [TestMethod]
    public void IsStandDownCleared_OnStandDownEndDate_ReturnsTrue()
    {
        // Arrange
        var eventDate = new DateTime(2024, 1, 1);
        var durationDays = 14;
        var standDownEvent = new StandDownEvent
        {
            EventDate = eventDate,
            DurationDays = durationDays
        };
        var bookingDate = eventDate.AddDays(durationDays); // Exactly when stand-down ends

        // Act
        var result = _service.IsStandDownCleared(standDownEvent, bookingDate);

        // Assert
        Assert.IsTrue(result, "Booking on stand-down end date should be eligible");
    }

    [TestMethod]
    public void IsStandDownCleared_OneDayAfterStandDownEnds_ReturnsTrue()
    {
        // Arrange
        var eventDate = new DateTime(2024, 1, 1);
        var durationDays = 14;
        var standDownEvent = new StandDownEvent
        {
            EventDate = eventDate,
            DurationDays = durationDays
        };
        var bookingDate = eventDate.AddDays(durationDays + 1); // One day after stand-down ends

        // Act
        var result = _service.IsStandDownCleared(standDownEvent, bookingDate);

        // Assert
        Assert.IsTrue(result, "Booking after stand-down ends should be eligible");
    }

    [TestMethod]
    public void IsStandDownCleared_DuringStandDown_ReturnsFalse()
    {
        // Arrange
        var eventDate = new DateTime(2024, 1, 1);
        var durationDays = 14;
        var standDownEvent = new StandDownEvent
        {
            EventDate = eventDate,
            DurationDays = durationDays
        };
        var bookingDate = eventDate.AddDays(7); // Middle of stand-down period

        // Act
        var result = _service.IsStandDownCleared(standDownEvent, bookingDate);

        // Assert
        Assert.IsFalse(result, "Booking during stand-down period should be ineligible");
    }

    #endregion
}
