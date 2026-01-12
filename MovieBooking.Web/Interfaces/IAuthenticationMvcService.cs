using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    
    public interface IAuthenticationMvcService
    {
        Task<AuthenticationViewModel?> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
    }

}
