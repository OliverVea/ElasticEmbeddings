using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

internal class QueryEmbeddingGeneratorService(ITextEmbeddingRepository textEmbeddingRepository) : IQueryEmbeddingGeneratorService
{
    public Task<Models.Embedding> GetEmbeddingAsync(string query)
    {
        return textEmbeddingRepository.GetEmbeddingAsync(query);
    }
}