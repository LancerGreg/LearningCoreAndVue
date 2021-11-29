namespace backend.Resources
{
    public class TwilioMessage
    {
        public static readonly string SidPending = "pending";
        public static string GetEmailComfirmMessage(string changePhoneNumberToken) => "\n\nFor comfirm your phone number enter this code:\n\n" + changePhoneNumberToken;
    }
}
