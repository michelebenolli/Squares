using AutoMapper;
using X.PagedList;

namespace Squares.Infrastructure.Mapping;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap(typeof(IPagedList<>), typeof(IPagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
    }
}
