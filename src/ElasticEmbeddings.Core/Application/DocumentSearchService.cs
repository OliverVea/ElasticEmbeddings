using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

internal class DocumentSearchService(
    IEmbeddingSearchProvider embeddingSearchProvider,
    IQueryEmbeddingGeneratorService queryEmbeddingGeneratorService)
    : IDocumentSearchService
{
    public async Task<DocumentSearchResult> SearchAsync(DocumentSearchRequest request)
    {
        var queryEmbedding = await queryEmbeddingGeneratorService.GetEmbeddingAsync(request.Query);

        var documentResults = await embeddingSearchProvider.SearchAsync(queryEmbedding);
        
        return new DocumentSearchResult(documentResults);
    }
}