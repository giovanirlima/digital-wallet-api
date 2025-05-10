using AutoMapper;
using Domain.DataTransferObjects.v1;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Command.Commands.v1.Users.UpdateUser;
using Infrastructure.Data.Database.Tables.v1;

namespace Api.Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserCommand, UserTable>(MemberList.None)
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
            .ForMember(dest => dest.AddressTable, opt => opt.MapFrom(src => src.Address));

        CreateMap<UpdateUserCommand, UserTable>(MemberList.None);

        CreateMap<UserTable, User>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressTable))
            .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => src.WalletTable))
            .ForMember(dest => dest.SentTransactions, opt => opt.MapFrom(src => src.SentTransaction))
            .ForMember(dest => dest.ReceivedTransactions, opt => opt.MapFrom(src => src.ReceivedTransaction));

        CreateMap<AddressTable, Address>(MemberList.None).ReverseMap();
        CreateMap<WalletTable, Wallet>(MemberList.None).ReverseMap();

        CreateMap<TransactionTable, Transaction>(MemberList.None)
            .ForMember(dest => dest.FromUsername, opt => opt.MapFrom(src => src.FromUser.Name))
            .ForMember(dest => dest.ToUsername, opt => opt.MapFrom(src => src.ToUser.Name));
    }
}