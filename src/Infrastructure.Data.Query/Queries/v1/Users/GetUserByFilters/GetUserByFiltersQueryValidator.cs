global using FluentValidation;

namespace Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;

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
    }
}