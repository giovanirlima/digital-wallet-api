using Digital.Wallet.Commands.v1.Wallets.AddTransaction;
using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Enums.v1;
using Digital.Wallet.Queries.v1.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Wallet.Endpoints.v1;

internal static class TransactionMap
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/transactions");

        group.MapGet("", async ([AsParameters] GetTransactionByFiltersQuery request, [FromServices] ISender sender) =>
        {
            var transactions = await sender.Send(request);

            return Results.Ok(transactions);
        })
        .RequireAuthorization()
        .WithName("GetTransactions")
        .Produces<IEnumerable<Transaction>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("", async ([FromBody] AddTransactionCommand request, [FromServices] ISender sender) =>
        {
            await sender.Send(request);

            return Results.Created("AddTransactiopn", $"Solicitacão de {GetTransactionType(request.Transaction)} criado com sucesso.");
        })
        .RequireAuthorization()
        .WithName("AddTransactions")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
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