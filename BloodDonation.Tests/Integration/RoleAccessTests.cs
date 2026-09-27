using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BloodDonation.Tests.Integration;

// TC17 and FR13: each role-restricted area rejects the wrong role.
// Signs in through the real login form using the Development seed users.
[TestClass]
public class RoleAccessTests
{
    private const string DonorEmail = "donor@test.local";
    private const string DonorPassword = "Donor#Test1";
    private const string StaffEmail = "staff@test.local";
    private const string StaffPassword = "Staff#Test1";
    private const string CoordinatorEmail = "coordinator@test.local";
    private const string CoordinatorPassword = "Coord#Test1";

    private const string StaffOutcomeUrl = "/Staff/Bookings/SetOutcome/1";

    private static WebApplicationFactory<Program> _factory = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _factory = new WebApplicationFactory<Program>();
        // Start the host once here; parallel tests would otherwise race to start it.
        _ = _factory.Server;
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestMethod]
    public async Task TC17_DonorOpensStaffOutcomeUrl_IsDenied()
    {
        var client = await SignedInClientAsync("/Account/Login", DonorEmail, DonorPassword);

        var response = await client.GetAsync(StaffOutcomeUrl);

        Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
        StringAssert.StartsWith(response.Headers.Location!.AbsolutePath, "/Account/AccessDenied");
    }

    [TestMethod]
    public async Task StaffOpensStaffOutcomeUrl_IsAllowed()
    {
        var client = await SignedInClientAsync("/Account/StaffLogin", StaffEmail, StaffPassword);

        var response = await client.GetAsync(StaffOutcomeUrl);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task AnonymousOpensStaffOutcomeUrl_RedirectsToLogin()
    {
        var client = NewClient();

        var response = await client.GetAsync(StaffOutcomeUrl);

        Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
        StringAssert.StartsWith(response.Headers.Location!.AbsolutePath, "/Account/Login");
    }

    [TestMethod]
    public async Task AnonymousOpensHomePage_IsAllowed()
    {
        var client = NewClient();

        var response = await client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    [DataRow("Donor", "/Donor/Dashboard", true)]
    [DataRow("Donor", "/Donor/Booking/Book", true)]
    [DataRow("Donor", "/Staff/Dashboard", false)]
    [DataRow("Donor", "/Coordinator/Dashboard", false)]
    [DataRow("Donor", "/Coordinator", false)]
    [DataRow("Staff", "/Staff/Dashboard", true)]
    [DataRow("Staff", "/Staff/Sessions", true)]
    [DataRow("Staff", "/Donor/Dashboard", false)]
    [DataRow("Staff", "/Coordinator/Dashboard", false)]
    [DataRow("Coordinator", "/Coordinator/Dashboard", true)]
    [DataRow("Coordinator", "/Coordinator", true)]
    [DataRow("Coordinator", "/Donor/Dashboard", false)]
    [DataRow("Coordinator", StaffOutcomeUrl, false)]
    public async Task RoleOpensArea_OnlyOwnAreaIsAllowed(string role, string url, bool allowed)
    {
        var client = role switch
        {
            "Donor" => await SignedInClientAsync("/Account/Login", DonorEmail, DonorPassword),
            "Staff" => await SignedInClientAsync("/Account/StaffLogin", StaffEmail, StaffPassword),
            _ => await SignedInClientAsync("/Account/StaffLogin", CoordinatorEmail, CoordinatorPassword),
        };

        var response = await client.GetAsync(url);

        if (allowed)
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        else
        {
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            StringAssert.StartsWith(response.Headers.Location!.AbsolutePath, "/Account/AccessDenied");
        }
    }

    [TestMethod]
    public async Task LoginWithWrongPassword_ShowsErrorAndStaysSignedOut()
    {
        var client = NewClient();

        var response = await PostLoginAsync(client, "/Account/Login", DonorEmail, "wrong-password");
        var body = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(body, "Invalid email or password.");
        var protectedPage = await client.GetAsync("/Donor/Dashboard");
        Assert.AreEqual(HttpStatusCode.Redirect, protectedPage.StatusCode);
        StringAssert.StartsWith(protectedPage.Headers.Location!.AbsolutePath, "/Account/Login");
    }

    private static HttpClient NewClient()
    {
        return _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    private static async Task<HttpClient> SignedInClientAsync(string loginUrl, string email, string password)
    {
        var client = NewClient();
        var response = await PostLoginAsync(client, loginUrl, email, password);
        Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, $"Login as {email} failed.");
        return client;
    }

    private static async Task<HttpResponseMessage> PostLoginAsync(HttpClient client, string loginUrl, string email, string password)
    {
        var loginPage = await client.GetStringAsync(loginUrl);
        var token = Regex.Match(loginPage, "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"").Groups[1].Value;

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = email,
            ["Password"] = password,
            ["__RequestVerificationToken"] = token,
        });

        return await client.PostAsync(loginUrl, form);
    }
}
