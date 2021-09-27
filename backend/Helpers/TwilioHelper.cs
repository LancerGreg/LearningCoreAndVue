using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public static class TwilioHelper
    {
        public static string TwilioAuthToken(this IConfiguration configuration) => configuration["Twilio:AuthToken"];
        public static string TwilioAccountSID(this IConfiguration configuration) => configuration["Twilio:AccountSID"];
        public static string TwilioPhone(this IConfiguration configuration) => configuration["Twilio:Phone"];
    }
}
