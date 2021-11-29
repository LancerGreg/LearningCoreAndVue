namespace backend.Managers
{
    public interface ISMTP
    {
        void SendSignUpRequest(string toEmail, string tokenVerified);
        void SendResetPasswordRequest(string toEmail, string tokenVerified);
    }
}
