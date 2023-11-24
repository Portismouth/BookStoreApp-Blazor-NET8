using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Author;
using BookStore.API.Models.Book;
using BookStore.API.Models.User;

namespace BookStore.API;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AuthorCreateDto, Author>().ReverseMap();
        CreateMap<AuthorUpdateDto, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
        CreateMap<Book, BookReadOnlyDto>()
            .ForMember(
                q => q.AuthorName,
                d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();

        CreateMap<Book, BookDetailsDto>()
               .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
               .ReverseMap();

        CreateMap<ApiUser, UserDto>().ReverseMap(); 
    }
}
