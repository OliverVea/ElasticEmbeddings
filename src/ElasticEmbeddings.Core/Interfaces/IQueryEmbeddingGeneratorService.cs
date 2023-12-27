namespace ElasticEmbeddings.Interfaces;

public interface IQueryEmbeddingGeneratorService
{
    Task<Models.Embedding> GetEmbeddingAsync(string query);
}