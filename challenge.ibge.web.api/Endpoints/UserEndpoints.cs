using challenge.ibge.authentication.Dtos;
using challenge.ibge.core.Services.Interfaces;
using challenge.ibge.web.api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace challenge.ibge.web.api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"/v1/users/register", async ([FromServices] IUserService userService, CreateUserDto userDto) =>
            {
                await userService.CreateAsync(userDto);
                return Results.Ok();
            })
            .AllowAnonymous()
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Create a new user entry",
                Description = "Use this route to create an user"
            })
            .Produces(200, typeof(CreateUserDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
        
        endpoints.MapPut("/v1/users{id:int}", async ([FromServices] IUserService userService,
                int id, UserDto userDto) =>
            {
                await userService.UpdateAsync(id, userDto);
                return Results.Ok();
            })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Update an user",
                Description = "Use this route to update an user"
            })
            .Produces(200, typeof(CreateUserDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
        
        endpoints.MapDelete("/v1/users{id:int}", async ([FromServices] IUserService userService,
                int id) =>
            {
                await userService.DeleteAsync(id);
                return Results.Ok();
            })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Delete an user",
                Description = "Use this route to delete an user"
            })
            .Produces(200)
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
    }
}                         