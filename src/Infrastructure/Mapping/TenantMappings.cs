using AutoMapper;
using Squares.Application.Multitenancy;
using Squares.Application.Multitenancy.Requests;
using Squares.Infrastructure.Multitenancy;

namespace Squares.Infrastructure.Mapping;

public class TenantMappings : Profile
{
    public TenantMappings()
    {
        CreateMap<AppTenant, TenantDto>();

        CreateMap<CreateTenantRequest, AppTenant>()
             .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id));

        CreateMap<UpdateTenantRequest, AppTenant>();
    }
}