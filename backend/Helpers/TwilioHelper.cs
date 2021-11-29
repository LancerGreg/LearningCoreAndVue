using Microsoft.Extensions.Configuration;

namespace backend.Helpers
{
    public static class TwilioHelper
    {
        public static string TwilioAuthToken(this IConfiguration configuration) => configuration["Twilio:AuthToken"];
        public static string TwilioAccountSID(this IConfiguration configuration) => configuration["Twilio:AccountSID"];
        public static string TwilioPhone(this IConfiguration configuration) => configuration["Twilio:Phone"];
    }
}
