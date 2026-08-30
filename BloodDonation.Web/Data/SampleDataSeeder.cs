using BloodDonation.Domain.Models;

namespace BloodDonation.Web.Data;

public static class SampleDataSeeder
{
    public static void Seed(SampleDataStore store)
    {
        if (store.Donors.Count > 0 || store.Appointments.Count > 0)
        {
            return;
        }

        var today = DateTime.Today;

        store.Donors.AddRange(new[]
        {
            new Donor { Id = 1, Name = "Alice Nguyen", DateOfBirth = new DateTime(1990, 5, 14), WeightKg = 68, LastDonationDate = today.AddDays(-90), Role = UserRole.Donor },
            new Donor { Id = 2, Name = "Ben Carter", DateOfBirth = new DateTime(1985, 11, 2), WeightKg = 82, LastDonationDate = null, Role = UserRole.Donor },
            new Donor { Id = 3, Name = "Chloe Davies", DateOfBirth = new DateTime(1972, 3, 22), WeightKg = 61, LastDonationDate = today.AddDays(-200), Role = UserRole.Donor },
            new Donor { Id = 4, Name = "Daniel Osei", DateOfBirth = new DateTime(2001, 7, 9), WeightKg = 75, LastDonationDate = today.AddDays(-84), Role = UserRole.Donor },
            new Donor { Id = 5, Name = "Emma Wallace", DateOfBirth = new DateTime(1965, 1, 30), WeightKg = 58, LastDonationDate = null, Role = UserRole.Donor },
            new Donor { Id = 6, Name = "Farid Khan", DateOfBirth = new DateTime(1998, 9, 17), WeightKg = 90, LastDonationDate = today.AddDays(-45), Role = UserRole.Donor },
            new Donor { Id = 7, Name = "Grace Liu", DateOfBirth = new DateTime(1955, 6, 11), WeightKg = 65, LastDonationDate = today.AddDays(-400), Role = UserRole.Donor },
            new Donor { Id = 8, Name = "Harry Thompson", DateOfBirth = new DateTime(2008, 12, 1), WeightKg = 55, LastDonationDate = null, Role = UserRole.Donor },
            new Donor { Id = 9, Name = "Isla Robertson", DateOfBirth = new DateTime(1980, 4, 25), WeightKg = 70, LastDonationDate = today.AddDays(-120), Role = UserRole.Donor },
            new Donor { Id = 10, Name = "Jack Ferreira", DateOfBirth = new DateTime(1993, 10, 8), WeightKg = 77, LastDonationDate = null, Role = UserRole.Donor },
            new Donor { Id = 11, Name = "Keira Sullivan", DateOfBirth = new DateTime(1960, 2, 19), WeightKg = 63, LastDonationDate = today.AddDays(-250), Role = UserRole.Donor },
            new Donor { Id = 12, Name = "Liam O'Brien", DateOfBirth = new DateTime(2003, 8, 30), WeightKg = 80, LastDonationDate = today.AddDays(-300), Role = UserRole.Donor }
        });

        store.Appointments.AddRange(new[]
        {
            new Appointment { Id = 1, DonorId = 1, SessionId = 1, CreatedDate = today.AddDays(-100), ScheduledDate = today.AddDays(-95), Status = AppointmentStatus.Donated },
            new Appointment { Id = 2, DonorId = 3, SessionId = 1, CreatedDate = today.AddDays(-85), ScheduledDate = today.AddDays(-70), Status = AppointmentStatus.Deferred },
            new Appointment { Id = 3, DonorId = 5, SessionId = 2, CreatedDate = today.AddDays(-60), ScheduledDate = today.AddDays(-50), Status = AppointmentStatus.Donated },
            new Appointment { Id = 4, DonorId = 7, SessionId = 2, CreatedDate = today.AddDays(-55), ScheduledDate = today.AddDays(-40), Status = AppointmentStatus.NoShow },
            new Appointment { Id = 5, DonorId = 9, SessionId = 1, CreatedDate = today.AddDays(-45), ScheduledDate = today.AddDays(-30), Status = AppointmentStatus.Donated },
            new Appointment { Id = 6, DonorId = 11, SessionId = 3, CreatedDate = today.AddDays(-40), ScheduledDate = today.AddDays(-20), Status = AppointmentStatus.Deferred },
            new Appointment { Id = 7, DonorId = 2, SessionId = 3, CreatedDate = today.AddDays(-30), ScheduledDate = today.AddDays(-15), Status = AppointmentStatus.Donated },
            new Appointment { Id = 8, DonorId = 4, SessionId = 1, CreatedDate = today.AddDays(-20), ScheduledDate = today.AddDays(-10), Status = AppointmentStatus.NoShow },
            new Appointment { Id = 9, DonorId = 6, SessionId = 2, CreatedDate = today.AddDays(-10), ScheduledDate = today.AddDays(5), Status = AppointmentStatus.Booked },
            new Appointment { Id = 10, DonorId = 8, SessionId = 3, CreatedDate = today.AddDays(-5), ScheduledDate = today.AddDays(10), Status = AppointmentStatus.Booked }
        });
    }
}
