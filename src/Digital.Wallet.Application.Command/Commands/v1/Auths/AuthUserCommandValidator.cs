using FluentValidation;

namespace Digital.Wallet.Commands.v1.Auths;

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