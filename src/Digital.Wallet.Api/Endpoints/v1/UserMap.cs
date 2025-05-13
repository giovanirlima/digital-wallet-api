using Digital.Wallet.Commands.v1.Users.AddUser;
using Digital.Wallet.Commands.v1.Users.DeleteUser;
using Digital.Wallet.Commands.v1.Users.UpdateUser;
using Digital.Wallet.DataTransferObjects.v1;
using Digital.Wallet.Queries.v1.Users.GetUserByFilters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Wallet.Endpoints.v1;

internal static class UserMap
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/users");

        group.MapGet("", async ([AsParameters] GetUserByFiltersQuery request, [FromServices] ISender sender) =>
        {
            var users = await sender.Send(request);

            return Results.Ok(users);
        })
        .RequireAuthorization()
        .WithName("GetUsers")
        .Produces<IEnumerable<User>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("", async ([FromBody] AddUserCommand request, ISender sender) =>
        {
            await sender.Send(request);

            return Results.Created();
        })
        .WithName("CreatedUser")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPut("{id}", async ([FromRoute] int id, [FromBody] UpdateUserCommand request, [FromServices] ISender sender) =>
        {
            await sender.Send(request.SetIdProperty(id));

            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("UpdatedUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("{id}", async ([FromRoute] int id, [FromServices] ISender sender) =>
        {
            await sender.Send(new DeleteUserCommand().SetIdProperty(id));

            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeletedUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}