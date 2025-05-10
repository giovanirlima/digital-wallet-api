using Domain.DataTransferObjects.v1;
using Infrastructure.Data.Command.Commands.v1.Auths.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.AuthEndpoints.v1;

public static class AuthEndpoint
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/authenticator");

        group.MapPost("", async ([FromBody] AuthUserCommand request, [FromServices] ISender sender) =>
        {
            var auth = await sender.Send(request);

            return Results.Ok(auth);
        })
        .WithName("AddToken")
        .Produces<IEnumerable<User>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}