global using FluentValidation;

namespace Digital.Wallet.Queries.v1.Users.GetUserByFilters;

public class GetUserByFiltersQueryValidator : AbstractValidator<GetUserByFiltersQuery>
{
    public GetUserByFiltersQueryValidator()
    {
        RuleForEach(x => x.Id)
            .GreaterThan(0)
                .WithMessage("Id do usuário deve ser maior que zero.");

        RuleForEach(x => x.Name)
            .Length(3, 50)
            .WithMessage("Nome do usuário deve ter entre 3 e 50 caracteres.");

        RuleForEach(x => x.Email)
            .EmailAddress()
                .WithMessage("Email do usuário deve ser um endereço de email válido.");

        //RuleFor(x => x.StartTransactionDate.HasValue)
        //    .NotEmpty()
        //    .When(x => x.EndTransactionDate.HasValue)
        //        .WithMessage("Data inicio de transação é obrigatória quando data fim é informada.");

        //RuleFor(x => x.EndTransactionDate.HasValue)
        //    .NotEmpty()
        //    .When(x => x.StartTransactionDate.HasValue)
        //        .WithMessage("Data fim de transação é obrigatória quando data inicio é informada.");
    }
}