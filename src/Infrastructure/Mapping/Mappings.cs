using AutoMapper;
using Squares.Application.Games.Requests;
using Squares.Domain.Games;
using X.PagedList;

namespace Squares.Infrastructure.Mapping;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<CreateGameRequest, Game>();

        CreateMap(typeof(IPagedList<>), typeof(IPagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
    }
}
