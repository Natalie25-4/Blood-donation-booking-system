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
            var age = _eligibilityService.CalculateAge(model.DateOfBirth, bookingDate);
            return age < EligibilityRules.MinimumAge
                ? new BookingIneligibleModel
                {
                    FailedRule = "Age",
                    Message = $"Donors must be at least {EligibilityRules.MinimumAge} years old.",
                    EarliestEligibleDate = _eligibilityService.MinimumAgeDate(model.DateOfBirth)
                }
                : new BookingIneligibleModel
                {
                    FailedRule = "Age",
                    Message = isNewDonor
                        ? $"New donors must be {EligibilityRules.NewDonorMaximumAge} or younger."
                        : $"Returning donors can donate until they turn {EligibilityRules.ReturningDonorAgeLimit}.",
                    EarliestEligibleDate = null
                };
        }

        if (!_eligibilityService.IsWeightEligible(model.WeightKg))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Weight",
                Message = $"Minimum weight for donation is {EligibilityRules.MinimumWeightKg}kg."
            };
        }

        if (!_eligibilityService.IsIntervalEligible(model.LastDonationDate, bookingDate))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Interval",
                Message = $"At least {EligibilityRules.MinimumDonationIntervalDays} days must pass between donations.",
                EarliestEligibleDate = _eligibilityService.EarliestDonationDate(model.LastDonationDate!.Value)
            };
        }

        if (!_eligibilityService.IsStandDownCleared(standDownEvent, bookingDate))
        {
            return new BookingIneligibleModel
            {
                FailedRule = "Stand-down",
                Message = $"Stand-down period for {standDownEvent!.Reason} has not elapsed.",
                EarliestEligibleDate = _eligibilityService.StandDownEndDate(standDownEvent)
            };
        }

        return null;
    }
}
