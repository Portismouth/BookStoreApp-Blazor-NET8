using BookStore.Blazor.Server.UI.Services.Base;

namespace BookStore.Blazor.Server.UI.Services;

public interface IAuthorsService
{
    Task<Response<List<AuthorReadOnlyDto>>> Get();
    Task<Response<AuthorDetailsDto>> Get(int id);
    Task<Response<AuthorUpdateDto>> GetForUpdate(int id);
    Task<Response<int>> Create(AuthorCreateDto author);
    Task<Response<int>> Edit(int id, AuthorUpdateDto author);
    Task<Response<int>> Delete(int id);
}
