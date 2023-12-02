using AutoMapper;
using BookStore.Blazor.Server.UI.Services.Base;

namespace BookStore.Blazor.Server.UI;
public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AuthorReadOnlyDto, AuthorUpdateDto>().ReverseMap();
    }
}

