﻿using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BookStore.Blazor.Server.UI.Services.Base;

public class BaseHttpService
{
    private readonly ILocalStorageService _localStorage;
    private readonly IClient _client;
    public BaseHttpService(IClient client, ILocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
    {
        if (apiException.StatusCode == 400)
        {
            return new Response<Guid>()
            {
                Message = "Validation errors have occurred.",
                ValidationErrors = apiException.Response,
                Success = false
            };
        }
        
        if (apiException.StatusCode == 404)
        {
            return new Response<Guid>()
            {
                Message = "The requested item could not be found.",
                Success = false
            };
        }

        if (apiException.StatusCode >= 200 && apiException.StatusCode <= 299)
        {
            return new Response<Guid>() { Message = "Operation Reported Success", Success = true };
        }

        return new Response<Guid>() { Message = "Something went wrong, please try again.", Success = false };
    }

    protected async Task GetBearerToken()
    {
        var token = await _localStorage.GetItemAsync<string>("accessToken");
        if (token != null)
        {
            _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}