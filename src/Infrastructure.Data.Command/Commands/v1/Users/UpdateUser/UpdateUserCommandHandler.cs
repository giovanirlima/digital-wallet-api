using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Command.Interfaces.v1;
using MediatR;

namespace Infrastructure.Data.Command.Commands.v1.Users.UpdateUser;

public class UpdateUserCommandHandler(IMapper mapper, IUserCommandRepository userCommandRepository) : IRequestHandler<UpdateUserCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserCommandRepository _userCommandRepository = userCommandRepository;

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userCommandRepository.GetUserByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var updateUser = _mapper.Map(request, user);

        await _userCommandRepository.UpdateUserAsync(updateUser, cancellationToken);
    }
}