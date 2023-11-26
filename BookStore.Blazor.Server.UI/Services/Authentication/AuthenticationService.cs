using Blazored.LocalStorage;
using BookStore.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.Blazor.Server.UI;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public AuthenticationService(IClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }
    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);

        //Store token
        await _localStorageService.SetItemAsync("accessToken", response.Token);

        // Change Auth State of app
        await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedIn();
        return true;
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedOut();
    }
}
