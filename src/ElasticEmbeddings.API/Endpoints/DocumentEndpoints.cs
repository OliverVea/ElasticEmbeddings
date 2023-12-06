using ElasticEmbeddings.API.Mappers;
using ElasticEmbeddings.API.Models;
using ElasticEmbeddings.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElasticEmbeddings.API.Endpoints;

public static class DocumentEndpoints
{
    public static void AddDocumentEndpoints(this WebApplication app)
    {
        app.MapPut("/documents/{id:guid}", async (
                DocumentPutRequest request,
                Guid id,
                CancellationToken cancellationToken,
                [FromServices] IDocumentService documentService,
                [FromServices] IDocumentMapper documentMapper) =>
            {
                var document = documentMapper.Map(request, id);
                
                await documentService.SetAsync(document, cancellationToken);
        
                return Results.Ok();
            })
            .WithName("PutDocument")
            .WithOpenApi();

        app.MapPost("documents", async (
                DocumentPostRequest request,
                CancellationToken cancellationToken,
                [FromServices] IDocumentService documentService,
                [FromServices] IDocumentMapper documentMapper) =>
            {
                var guid = Guid.NewGuid();
                var document = documentMapper.Map(request, guid);

                await documentService.SetAsync(document, cancellationToken);

                return guid;
            })
            .WithName("PostDocument")
            .WithOpenApi();

        app.MapGet("documents", async (
                CancellationToken cancellationToken,
                [FromServices] IDocumentService documentService) =>
            {
                return (await documentService.ListIdsAsync(cancellationToken)).Select(x => x.Value).ToArray();

            })
            .WithName("GetDocumentIds")
            .WithOpenApi();
    }
}