using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace backend.Managers
{
    public class SMTP : ISMTP
    {
        private readonly string appName = "";
        private readonly string appLocalURL = "";
        private readonly string smtpServer = "";
        private readonly string from = "";
        private readonly string pass = "";

        private readonly IConfiguration _configuration;
        private readonly SmtpClient client;

        public SMTP(IConfiguration configuration)
        {
            _configuration = configuration;
            appName = _configuration["AppSettings:AppName"];
            appLocalURL = _configuration["AppSettings:AppLocalURL"];
            smtpServer = _configuration["CredentialsSMTP:Server"];
            from = _configuration["CredentialsSMTP:Login"];
            pass = _configuration["CredentialsSMTP:Password"];

            client = new SmtpClient(smtpServer);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;
        }

        public void SendSignUpRequest(string toEmail, string tokenVerified)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(from, appName);
            mail.To.Add(toEmail);
            mail.Subject = "Confirm email on " + appName;
            mail.Body = "<a href='" + appLocalURL + "account/confirm_email?email=" + toEmail + "&token=" + tokenVerified + "' class='myButton'>Confirm Email</a>";
            mail.IsBodyHtml = true;

            client.Send(mail);
        }

        public void SendResetPasswordRequest(string toEmail, string tokenVerified)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(from, appName);
            mail.To.Add(toEmail);
            mail.Subject = "Reset passwrod on  " + appName;
            mail.Body = "<a href='" + appLocalURL + "account/confirm_reset_password?email=" + toEmail + "&token=" + tokenVerified + "' class='myButton'>Confirm Reset Password</a>";
            mail.IsBodyHtml = true;

            client.Send(mail);
        }
    }
}
