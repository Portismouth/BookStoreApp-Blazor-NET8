using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Author;

namespace BookStore.API;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AuthorCreateDto, Author>().ReverseMap();
        CreateMap<AuthorUpdateDto, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
    }
}
