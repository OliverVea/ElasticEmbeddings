using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

internal class QueryEmbeddingGeneratorService(ITextEmbeddingProvider textEmbeddingProvider) : IQueryEmbeddingGeneratorService
{
    public Task<Embedding> GetEmbeddingAsync(string query)
    {
        return textEmbeddingProvider.GetEmbeddingAsync(query);
    }
}