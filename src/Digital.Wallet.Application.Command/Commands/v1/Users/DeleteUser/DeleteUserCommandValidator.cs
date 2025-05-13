using FluentValidation;

namespace Digital.Wallet.Commands.v1.Users.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
                .WithMessage("Id do usuário é obrigatório.")
            .GreaterThan(0)
                .WithMessage("Id do usuário deve ser maior que 0.");
    }
}