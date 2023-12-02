using BookStore.API.Models.Book;

namespace BookStore.API;

public class AuthorDetailsDto : AuthorReadOnlyDto
{
    public List<BookReadOnlyDto> Books { get; set; }
}
