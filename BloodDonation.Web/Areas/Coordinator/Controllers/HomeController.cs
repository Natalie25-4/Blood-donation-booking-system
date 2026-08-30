using BloodDonation.Domain;
using BloodDonation.Domain.Models;
using BloodDonation.Web.Areas.Coordinator.Models;
using BloodDonation.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Areas.Coordinator.Controllers;

[Area("Coordinator")]
public class HomeController : Controller
{
    private readonly AppointmentAnalyticsService _analyticsService;
    private readonly SampleDataStore _store;

    public HomeController(AppointmentAnalyticsService analyticsService, SampleDataStore store)
    {
        _analyticsService = analyticsService;
        _store = store;
    }

    public IActionResult Index()
    {
        var analytics = _analyticsService.Calculate(_store.Appointments);

        var countsByStatus = Enum.GetValues<AppointmentStatus>()
            .Select(status => (status, _store.Appointments.Count(a => a.Status == status)))
            .ToList();

        var model = new DashboardViewModel
        {
            DeferralRatePercentage = analytics.DeferralRatePercentage,
            AverageBookingLeadTimeDays = analytics.AverageBookingLeadTimeDays,
            AppointmentCountsByStatus = countsByStatus,
            TotalAppointments = _store.Appointments.Count
        };

        return View(model);
    }
}
