using Digital.Wallet.Validators.v1;
using FluentValidation;

namespace Digital.Wallet.Commands.v1.Users.AddUser;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Nome do usuário é obrigatório.")
            .Length(3, 50)
                .WithMessage("Nome do usuário deve ter entre 3 e 50 caracteres.");

        RuleFor(x => x.Birthday)
            .NotEmpty()
                .WithMessage("Data de nascimento é obrigatória.");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email do usuário é obrigatório.")
            .EmailAddress()
                .WithMessage("Email informado é inválido.");

        RuleFor(x => x.Address)
            .NotNull()
                .WithMessage("Endereço do usuário é obrigatório.")
            .SetValidator(new AddressValidator());
    }
}