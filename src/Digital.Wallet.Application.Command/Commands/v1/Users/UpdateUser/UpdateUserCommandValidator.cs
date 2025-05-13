using FluentValidation;

namespace Digital.Wallet.Commands.v1.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
                .WithMessage("Id do usuário é obrigatório.")
            .GreaterThan(0)
                .WithMessage("Id do usuário deve ser maior que 0");

        RuleFor(x => x.Name)
            .Length(3, 50)
                .WithMessage("Nome do usuário deve ter entre 3 e 50 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress()
                .WithMessage("Email do usuário é inválido.");
    }
}