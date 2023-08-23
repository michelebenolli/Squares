using AutoMapper;
using Squares.Application.Auditing;
using Squares.Application.Identity.Roles;
using Squares.Application.Identity.Users;
using Squares.Application.Identity.Users.Requests;
using Squares.Domain.Identity;
using Squares.Infrastructure.Auditing;
using Squares.Infrastructure.Identity;

namespace Squares.Infrastructure.Mapping;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();

        CreateMap<User, ApplicationUserDto>();
        CreateMap<ApplicationUser, ApplicationUserDto>()
            .IncludeMembers(x => x.User);

        CreateMap<CreateUserRequest, User>();
        CreateMap<CreateUserRequest, ApplicationUser>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

        CreateMap<UpdateUserRequest, User>();
        CreateMap<UpdateUserRequest, ApplicationUser>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

        CreateMap<Trail, TrailDto>();
    }
}