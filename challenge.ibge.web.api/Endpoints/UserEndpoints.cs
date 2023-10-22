using challenge.ibge.authentication.Dtos;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.web.api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace challenge.ibge.web.api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"/v1/users", async ([FromServices] IUserService userService, CreateUserDto userDto) =>
            {
                await userService.CreateAsync(userDto);
                return Results.Ok();
            })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Create a new user entry",
                Description = "Use this route to create an user"
            })
            .Produces(200, typeof(CreateUserDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
    }
}                         