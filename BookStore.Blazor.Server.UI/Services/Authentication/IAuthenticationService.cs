using BookStore.Blazor.Server.UI.Services.Base;

namespace BookStore.Blazor.Server.UI;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(LoginUserDto loginModel);
    Task Logout();
}
