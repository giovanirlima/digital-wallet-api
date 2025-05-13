using Digital.Wallet.DataTransferObjects.v1;
using FluentValidation;

namespace Digital.Wallet.Validators.v1;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
                .WithMessage("Rua é obrigatória.")
            .Length(3, 50)
                .WithMessage("Rua deve ter entre 3 e 50 caracteres.");

        RuleFor(x => x.Number)
            .NotEmpty()
                .WithMessage("Número é obrigatório.")
            .GreaterThanOrEqualTo(0)
                .WithMessage("Número da informado é inválido.");

        RuleFor(x => x.City)
            .NotEmpty()
                .WithMessage("Cidade é obrigatória.")
            .Length(3, 50)
                .WithMessage("Cidade deve ter entre 3 e 50 caracteres.");

        RuleFor(x => x.Country)
            .NotEmpty()
                .WithMessage("País é obrigatório.")
            .Length(3, 50)
                .WithMessage("País deve ter entre 3 e 50 caracteres.");
    }
}