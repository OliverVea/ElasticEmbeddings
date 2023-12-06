using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticEmbeddings.API.Endpoints;

public static class SearchEndpoints
{
    public static void AddSearchEndpoints(this WebApplication app)
    {
        app.MapGet("/search/{query}", async (string query, [FromServices] IDocumentSearchService searchService) =>
            {
                var result = await searchService.SearchAsync(new DocumentSearchRequest
                {
                    Query = query
                });
        
                return Results.Ok(result);
            })
            .WithName("Search")
            .WithOpenApi();
    }
}