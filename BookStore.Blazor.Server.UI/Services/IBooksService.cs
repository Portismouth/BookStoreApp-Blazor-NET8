using BookStore.Blazor.Server.UI.Services.Base;

namespace BookStore.Blazor.Server.UI.Services;

public interface IBooksService
{
    Task<Response<List<BookReadOnlyDto>>> Get();
    Task<Response<BookDetailsDto>> Get(int id);
    Task<Response<BookUpdateDto>> GetForUpdate(int id);
    Task<Response<int>> Create(BookCreateDto author);
    Task<Response<int>> Edit(int id, BookUpdateDto author);
    Task<Response<int>> Delete(int id);
}