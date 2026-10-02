using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using BloodDonation.Domain;
using BloodDonation.Domain.Models;
using BloodDonation.Web.Areas.Donor.Models;
using BloodDonation.Web.Data;
using Microsoft.AspNetCore.Mvc;
using DonorModel = BloodDonation.Domain.Models.Donor;

namespace BloodDonation.Web.Areas.Donor.Controllers;

[Area("Donor")]
[Authorize(Roles = Roles.Donor)]
public class BookingController : Controller
{
    private readonly EligibilityService _eligibilityService;
    private readonly SampleDataStore _store;

    public BookingController(EligibilityService eligibilityService, SampleDataStore store)
    {
        _eligibilityService = eligibilityService;
        _store = store;
    }

    [HttpGet]
    public IActionResult Book()
    {
        return View(new BookingFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Book(BookingFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var bookingDate = DateTime.Today;
        var isNewDonor = model.LastDonationDate is null;

        StandDownEvent? standDownEvent = null;
        if (model.HasStandDownEvent
            && model.StandDownReason.HasValue
            && model.StandDownEventDate.HasValue
            && model.StandDownDurationDays.HasValue)
        {
            standDownEvent = new StandDownEvent
            {
                Reason = model.StandDownReason.Value,
                EventDate = model.StandDownEventDate.Value,
                DurationDays = model.StandDownDurationDays.Value
            };
        }

        var ineligible = Evaluate(model, bookingDate, isNewDonor, standDownEvent);
        if (ineligible is not null)
        {
            return View("Ineligible", ineligible);
        }

        var donor = new DonorModel
        {
            Id = _store.Donors.Count + 1,
            Name = "Guest Donor",
            DateOfBirth = model.DateOfBirth,
            WeightKg = model.WeightKg,
            LastDonationDate = model.LastDonationDate,
            Role = UserRole.Donor
        };
        _store.Donors.Add(donor);

        var appointment = new Appointment
        {
            Id = _store.Appointments.Count + 1,
            DonorId = donor.Id,
            SessionId = 0,
            CreatedDate = DateTime.Now,
            ScheduledDate = bookingDate,
            Status = AppointmentStatus.Booked
        };
        _store.Appointments.Add(appointment);

        return View("Confirmation", appointment);
    }

    private BookingIneligibleModel? Evaluate(
        BookingFormModel model,
        DateTime bookingDate,
        bool isNewDonor,
        StandDownEvent? standDownEvent)
    {
        if (!_eligibilityService.IsAgeEligible(model.DateOfBirth, bookingDate, isNewDonor))
        {
            var age = CalculateAge(model.DateOfBirth, bookingDate);
            return age < 16
                ? new BookingIneligibleModel
                {
                    FailedRule = "Age",
                    Message = "Donors must be at least 16 years old.",
                    EarliestEligibleDate = model.DateOfBirth.AddYears(16)
                }
                : new BookingIneligibleModel
                {
                    FailedRule = "Age",
                    Message = isNewDonor
                        ? "New donors must be 71 or younger."
                        : "Returning donors can donate up to their 81st birthday.",
                    EarliestEligibleDate = null
                };
        }

        if (!_eligibilityService.IsWeightEligible(model.WeightKg))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Weight",
                Message = "Minimum weight for donation is 50kg."
            };
        }

        if (!isNewDonor && !_eligibilityService.IsIntervalEligible(model.LastDonationDate!.Value, bookingDate))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Interval",
                Message = "At least 84 days must pass between donations.",
                EarliestEligibleDate = model.LastDonationDate.Value.AddDays(84)
            };
        }

        if (!_eligibilityService.IsStandDownCleared(standDownEvent!, bookingDate))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Stand-down",
                Message = $"Stand-down period for {standDownEvent!.Reason} has not elapsed.",
                EarliestEligibleDate = standDownEvent.EventDate.AddDays(standDownEvent.DurationDays)
            };
        }

        return null;
    }

    private static int CalculateAge(DateTime dateOfBirth, DateTime onDate)
    {
        var age = onDate.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > onDate.AddYears(-age)) age--;
        return age;
    }
}
