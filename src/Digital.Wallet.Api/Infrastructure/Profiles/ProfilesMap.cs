using AutoMapper;
using Digital.Wallet.Commands.v1.Users.AddUser;
using Digital.Wallet.Commands.v1.Users.UpdateUser;
using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Services.v1;
using Digital.Wallet.Tables.v1;

namespace Digital.Wallet.Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserCommand, UserTable>(MemberList.None)
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Encrypt()))
            .ForMember(dest => dest.AddressTable, opt => opt.MapFrom(src => src.Address));

        CreateMap<UpdateUserCommand, UserTable>(MemberList.None);

        CreateMap<UserTable, User>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressTable))
            .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => src.WalletTable))
            .ForMember(dest => dest.SentTransactions, opt => opt.MapFrom(src => src.SentTransaction))
            .ForMember(dest => dest.ReceivedTransactions, opt => opt.MapFrom(src => src.ReceivedTransaction));

        CreateMap<AddressTable, Address>(MemberList.None).ReverseMap();
        CreateMap<WalletTable, MyWallet>(MemberList.None).ReverseMap();

        CreateMap<TransactionTable, Transaction>(MemberList.None)
            .ForMember(dest => dest.FromUsername, opt => opt.MapFrom(src => src.FromUser.Name))
            .ForMember(dest => dest.ToUsername, opt => opt.MapFrom(src => src.ToUser.Name));
    }
}