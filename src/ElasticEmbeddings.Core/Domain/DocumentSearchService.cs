using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

internal class DocumentSearchService(
    IEmbeddingSearchRepository embeddingSearchRepository,
    IQueryEmbeddingGeneratorService queryEmbeddingGeneratorService)
    : IDocumentSearchService
{
    public async Task<DocumentSearchResult> SearchAsync(DocumentSearchRequest request)
    {
        var queryEmbedding = await queryEmbeddingGeneratorService.GetEmbeddingAsync(request.Query);

        var documentResults = await embeddingSearchRepository.SearchAsync(queryEmbedding);
        
        return new DocumentSearchResult(documentResults);
    }
}