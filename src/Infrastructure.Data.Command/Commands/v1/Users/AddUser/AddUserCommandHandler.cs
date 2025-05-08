using AutoMapper;
using CrossCutting.Exceptions;
using Domain.Entities.v1;
using Infrastructure.Data.Command.Interfaces.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.AddUser;

public class AddUserCommandHandler(IMapper mapper, IUserCommandRepository userCommandRepository) : IRequestHandler<AddUserCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserCommandRepository _userCommandRepository = userCommandRepository;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userCommandRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (userExists is not null)
            throw new BadRequestException("Email informado já está cadastrado.");

        var user = _mapper.Map<User>(request);

        await _userCommandRepository.AddUserAsync(user, cancellationToken);
    }
}