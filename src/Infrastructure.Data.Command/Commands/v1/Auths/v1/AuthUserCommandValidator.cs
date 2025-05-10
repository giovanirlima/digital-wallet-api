using FluentValidation;

namespace Infrastructure.Data.Command.Commands.v1.Auths.v1;

public class AuthUserCommandValidator : AbstractValidator<AuthUserCommand>
{
    public AuthUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email é obrigatório.")
            .EmailAddress()
                .WithMessage("Email informado é inválido");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Senha é obrigatória")
            .MaximumLength(100)
                .WithMessage("Senha atingiu o máximo de caracteres.");
    }
}