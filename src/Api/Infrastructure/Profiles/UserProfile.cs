using AutoMapper;
using Domain.Entities.v1;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Command.Commands.v1.Users.UpdateUser;

namespace Api.Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserCommand, User>(MemberList.None);
        CreateMap<UpdateUserCommand, User>(MemberList.None)
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom<NameUpdateResolver>())
            .ForMember(dest => dest.Email, opt => opt.MapFrom<NameUpdateResolver>())
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom<NameUpdateResolver>())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}

public class NameUpdateResolver
    : IValueResolver<UpdateUserCommand, User, string>,
      IValueResolver<UpdateUserCommand, User, DateTime>
{
    public string Resolve(UpdateUserCommand source, User destination, string destMember, ResolutionContext context) =>
        source.Name ?? destination.Name;

    public DateTime Resolve(UpdateUserCommand source, User destination, DateTime destMember, ResolutionContext context) =>
        source.Birthday ?? DateTime.SpecifyKind(destination.Birthday, DateTimeKind.Utc);
}