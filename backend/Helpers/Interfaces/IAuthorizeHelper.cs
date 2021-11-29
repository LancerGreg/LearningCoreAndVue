namespace backend.Helpers.Interfaces
{
    public interface IAuthorizeHelper
    {
        bool OnAuthorization();
        void Authorization(string email);
        void LogoutAuthorization();
    }
}
