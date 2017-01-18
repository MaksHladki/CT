using System;
using System.Collections.Generic;
using System.Security.Authentication;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TelemetryClientSample
{
    class Program
    {
        static void Main(string[] args)
        {

            ApplicationInsight.Instance.TrackPageView("Home");
            ApplicationInsight.Instance.TrackEvent("Redirect", new Dictionary<string, string> { { "From", "Home" }, { "To", "Account" } });

            ApplicationInsight.Instance.TrackPageView("Account");
            ApplicationInsight.Instance.TrackEvent("Login");
            ApplicationInsight.Instance.TrackException(new AuthenticationException("User not found"));
            ApplicationInsight.Instance.TrackEvent("Redirect", new Dictionary<string, string> { { "From", "Account" }, { "To", "Shop" }, {"DailyKey" , "DSJU-WERDT-9803"} });

            ApplicationInsight.Instance.TrackPageView("Shop");

            ApplicationInsight.Instance.TrackTrace("Overtime", 100);

            ApplicationInsight.Instance.Flush();
        }
    }

    public class ApplicationInsight
    {
        private static readonly Lazy<ApplicationInsight> _instance = new Lazy<ApplicationInsight>(() => new ApplicationInsight());

        private readonly TelemetryClient _tc;

        private ApplicationInsight()
        {
            _tc = InitializeTelemetry();

        }

        public static ApplicationInsight Instance
        {
            get { return _instance.Value; }
        }

        private TelemetryClient InitializeTelemetry()
        {
            var tc = new TelemetryClient
            {
                InstrumentationKey =
                    @"clientNotification-ddadd7d9-4bd5-411a-ab5c-c1fe3322e005;a518aea0-2971-4fd4-9c38-595b8ffee7ab;a518aea0-2971-4fd4-9c38-595b8ffee7ab"
            };

            // Set session data:
            tc.Context.User.Id = Environment.UserName;
            tc.Context.Session.Id = Guid.NewGuid().ToString();
            tc.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

            return tc;
        }

        public void TrackPageView(string name)
        {
            _tc.TrackPageView(name);
        }

        public void TrackException(Exception ex)
        {
            _tc.TrackException(ex);
        }

        public void Track(ITelemetry telemetry)
        {
            _tc.Track(telemetry);
        }

        public void Flush()
        {
            _tc.Flush();
        }

        public void TrackEvent(string eventName, Dictionary<string, string> options = null)
        {
            _tc.TrackEvent(eventName, options);
        }

        //public void TrackEvent(string eventName)
        //{
        //    _tc.TrackAvailability();
        //}

        public void TrackTrace(string eventName, int number)
        {
            _tc.TrackTrace(
                "The number is even",
                SeverityLevel.Warning,
                new Dictionary<string, string> { { "number", number.ToString() } }
            );
        }
    }

    public class CustomTelemetry : ITelemetry
    {
        public void Sanitize()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset Timestamp { get; set; }
        public TelemetryContext Context { get; }
        public string Sequence { get; set; }
    }
}
