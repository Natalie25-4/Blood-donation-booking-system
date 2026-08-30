using BloodDonation.Domain.Models;
using BloodDonation.Web.Areas.Staff.Models;
using BloodDonation.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Staff.Controllers;

[Area("Staff")]
public class BookingsController : Controller
{
    private readonly SampleDataStore _store;

    public BookingsController(SampleDataStore store)
    {
        _store = store;
    }

    public IActionResult Index()
    {
        var items = _store.Appointments
            .OrderBy(a => a.ScheduledDate)
            .Select(a => new BookingListItem
            {
                Appointment = a,
                DeferralReason = _store.DeferralReasons.GetValueOrDefault(a.Id)
            })
            .ToList();

        return View(items);
    }

    [HttpGet]
    public IActionResult SetOutcome(int id)
    {
        var appointment = _store.Appointments.FirstOrDefault(a => a.Id == id);
        if (appointment is null)
        {
            return NotFound();
        }

        var model = new SetOutcomeFormModel
        {
            AppointmentId = id,
            Reason = _store.DeferralReasons.GetValueOrDefault(id)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetOutcome(SetOutcomeFormModel model)
    {
        var appointment = _store.Appointments.FirstOrDefault(a => a.Id == model.AppointmentId);
        if (appointment is null)
        {
            return NotFound();
        }

        if (model.Outcome == AppointmentStatus.Deferred && string.IsNullOrWhiteSpace(model.Reason))
        {
            ModelState.AddModelError(nameof(model.Reason), "A reason is required when deferring a donor.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        appointment.Status = model.Outcome!.Value;

        if (model.Outcome == AppointmentStatus.Deferred)
        {
            _store.DeferralReasons[appointment.Id] = model.Reason!.Trim();
        }
        else
        {
            _store.DeferralReasons.Remove(appointment.Id);
        }

        return RedirectToAction(nameof(Index));
    }
}
