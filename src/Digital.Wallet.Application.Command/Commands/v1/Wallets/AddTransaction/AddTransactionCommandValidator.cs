using FluentValidation;

namespace Digital.Wallet.Commands.v1.Wallets.AddTransaction;

public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    public AddTransactionCommandValidator()
    {
        RuleFor(x => x.FromUserId)
            .NotEmpty()
                .WithMessage("O campo com id do usúario de envio é obrigatório.");

        RuleFor(x => x.Transaction)
            .IsInEnum()
                .WithMessage("O campo tipo de transação é obrigatório.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
                .WithMessage("O campo valor deve ser maior que zero.");
    }
}