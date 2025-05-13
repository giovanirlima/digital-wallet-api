

using Digital.Wallet.Commands.v1.Auths;
using Digital.Wallet.DataTransferObjects.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Wallet.Endpoints.v1;

internal static class AuthenticatorMap
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/authenticators");

        group.MapPost("", async ([FromBody] AuthUserCommand request, [FromServices] ISender sender) =>
        {
            var token = await sender.Send(request);

            return Results.Ok(token);
        })
        .WithName("AddToken")
        .Produces<AuthToken>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}