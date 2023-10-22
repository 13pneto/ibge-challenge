namespace challenge.ibge.web.api.Endpoints;

public static class IWebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapLocalityEndpoints();
        app.MapUserEndpoints();
        app.MapAuthenticationEndpoints();
    }
}