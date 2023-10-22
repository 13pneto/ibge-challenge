using challenge.ibge.infra.data.Dtos;
using challenge.ibge.infra.data.Services.Interfaces;
using challenge.ibge.web.api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace challenge.ibge.web.api.Endpoints;

public static class LocalityEndpoints
{
    public static void MapLocalityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/v1/localities", async (LocalityDto localityDto, ILocalityService localityService) =>
            {
                var result = await localityService.CreateAsync(localityDto);
                return Results.Ok(result);
            })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Create a new locality entry",
                Description = "Use this route to create an locality"
            })
            .Produces(200, typeof(LocalityDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));

        
        endpoints.MapPut("/v1/localities/{id:int}", async ([FromRoute]int id, [FromBody] LocalityDto localityDto,
            ILocalityService localityService) =>
        {
            var result = await localityService.UpdateAsync(id, localityDto);
            return Results.Ok(result);
        })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Update a locality resource by ID",
                Description = "Use this route to update a locality resource based on its ID"
            })
            .Produces(200, typeof(LocalityDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));

        
        endpoints.MapDelete("/v1/localities/{id:int}", async ([FromRoute] int id,
            ILocalityService localityService) =>
        {
            await localityService.DeleteAsync(id);
            return Results.Ok();
        })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Delete a locality entry",
                Description = "Use this route to delete a locality"
            })
            .Produces(200)
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));

        endpoints.MapGet("/v1/localities", async ([FromQuery] string? filter, ILocalityService localityService) =>
        {
            var result = await localityService.GetAllAsync(filter);
            return Results.Ok(result);
        })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Get all localities",
                Description = "Use this route to get all localities"
            })
            .Produces(200, typeof(List<LocalityDto>))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));;

        
        endpoints.MapGet("/v1/localities/{id:int}", async ([FromRoute] int id,
            ILocalityService localityService) =>
        {
            var result = await localityService.GetByIdAsync(id);
            return Results.Ok(result);
        })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Get a locality by id",
                Description = "Use this route to get a locaity by id"
            })
            .Produces(200, typeof(LocalityDto))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));

        
        endpoints.MapPost("/v1/localities/import", async ([FromForm] IFormFile file,
            ILocalityService localityService) =>
        {
            var result = await localityService.ImportAsync(file);
            return Results.Ok(result);
        })
            .WithMetadata(new SwaggerOperationAttribute()
            {
                Summary = "Import localities of an excel file (.xls / .xlxs)",
                Description = "Use this route to import locaties using an .xls file \n" +
                              "Important: Create the .xls file using format: https://github.com/andrebaltieri/ibge/blob/main/SQL%20INSERTS%20-%20API%20de%20localidades%20IBGE.xlsx"
            })
            .Produces(200, typeof(LocalityImportResult))
            .Produces(400)
            .Produces(500, typeof(InternalErrorException));
    }
}                           