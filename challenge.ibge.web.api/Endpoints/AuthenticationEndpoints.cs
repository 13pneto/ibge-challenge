using challenge.ibge.authentication.Dtos;
using challenge.ibge.authentication.Services;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.web.api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace challenge.ibge.web.api.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/v1/authentication/login",
                async ([FromServices] IUserService userService, LoginDto loginDto) =>
                {
                    var authenticateUserDto = await userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                    if (authenticateUserDto is null)
                    {
                        return Results.BadRequest("Usuário/senha inválido(s)");
                    }
                    
                    return Results.Ok(TokenService.GenerateToken(authenticateUserDto));
                })
            .AllowAnonymous()
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Request login",
                Description = "Use this route to login in application"
            })
            .Produces(200, typeof(TokenDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
    }
}                         