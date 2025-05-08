using Domain.Entities.v1;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Command.Commands.v1.Users.DeleteUser;
using Infrastructure.Data.Command.Commands.v1.Users.UpdateUser;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.UserMaps.v1;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/v1/users");

        group.MapGet("", async ([AsParameters] GetUserByFiltersQuery request, [FromServices] ISender sender) =>
        {
            var users = await sender.Send(request);

            return Results.Ok(users);
        })
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
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapPut("{id}", async ([FromRoute] int id, [FromBody] UpdateUserCommand request, [FromServices] ISender sender) =>
        {
            await sender.Send(request.SetIdProperty(id));

            return Results.NoContent();
        })
        .WithName("UpdatedUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);

        group.MapDelete("{id}", async ([FromRoute] int id, [FromServices] ISender sender) =>
        {
            await sender.Send(new DeleteUserCommand().SetIdProperty(id));

            return Results.NoContent();
        })
        .WithName("DeletedUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}