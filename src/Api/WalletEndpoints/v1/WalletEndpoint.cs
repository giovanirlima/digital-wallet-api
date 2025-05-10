using Domain.Enums.v1;
using Infrastructure.Data.Command.Commands.v1.Wallets.AddTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.WalletEndpoints.v1;

public static class WalletEndpoint
{
    public static void MapWalletEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/wallets");

        group.MapPost("payment", async ([FromBody] AddTransactionCommand request, ISender sender) =>
        {
            await sender.Send(request);

            return Results.Created("AddTransactiopn", $"Solicitacão de {GetTransactionType(request.Transaction)} criado com sucesso.");
        })
        .RequireAuthorization()
        .WithName("AddTransaction")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }

    private static string GetTransactionType(TransactionType transactionType) =>
        transactionType switch
        {
            TransactionType.Transfer => "Transferência",
            TransactionType.Withdraw => "Débito",
            _ => "Depósito"
        };
}