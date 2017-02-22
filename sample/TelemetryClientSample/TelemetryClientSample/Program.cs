using System;
using System.Collections.Generic;
using System.Security.Authentication;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace TelemetryClientSample
{
    class Program
    {
        static void Main()
        {
            ApplicationInsight.Instance.TrackPageView("Home");
            ApplicationInsight.Instance.TrackEvent("Redirect",
                new Dictionary<string, string> { { "From", "Home" }, { "To", "Account" } });

            ApplicationInsight.Instance.TrackPageView("Account");
            ApplicationInsight.Instance.TrackEvent("Login");
            ApplicationInsight.Instance.TrackException(new AuthenticationException("User not found"));
            ApplicationInsight.Instance.TrackEvent("Redirect",
                new Dictionary<string, string>
                {
                    { "From", "Account" },
                    { "To", "Shop" },
                    { "DailyKey" , "DSJU-WERDT-9803"}
                });

            ApplicationInsight.Instance.TrackPageView("Shop");
            ApplicationInsight.Instance.TrackTrace("Overtime", 100);
            ApplicationInsight.Instance.TrackMetric("Total count", 32);

            ApplicationInsight.Instance.Flush();
        }
    }

    internal class ApplicationInsight
    {
        #region Fields 

        private const string Key = @"11d0404f-5be6-4d86-a3ce-311cd10b1b0f";

        private static readonly Lazy<ApplicationInsight> _instance
            = new Lazy<ApplicationInsight>(() => new ApplicationInsight());

        private readonly TelemetryClient _tc;

        #endregion

        #region Constructors

        private ApplicationInsight()
        {
            _tc = InitializeTelemetry();
        }

        #endregion

        #region Properties

        public static ApplicationInsight Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region Methods

        private TelemetryClient InitializeTelemetry()
        {
            var tc = new TelemetryClient
            {
                InstrumentationKey = Key
            };

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

        public void TrackEvent(string eventName, Dictionary<string, string> options = null)
        {
            _tc.TrackEvent(eventName, options);
        }

        public void TrackMetric(string metricName, int value)
        {
            _tc.TrackMetric(metricName, value);
        }

        public void TrackTrace(string eventName, int number)
        {
            _tc.TrackTrace(
                "The number is even",
                SeverityLevel.Warning,
                new Dictionary<string, string> { { "number", number.ToString() } }
            );
        }

        public void Flush()
        {
            _tc.Flush();
        }

        #endregion
    }
}
