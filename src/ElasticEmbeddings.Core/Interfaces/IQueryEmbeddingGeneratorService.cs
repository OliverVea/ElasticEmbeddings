using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IQueryEmbeddingGeneratorService
{
    Task<Embedding> GetEmbeddingAsync(string query);
}