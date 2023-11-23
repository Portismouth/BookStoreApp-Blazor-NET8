using BookStore.API.Models;

namespace BookStore.API;

public class AuthorReadOnlyDto : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
}
