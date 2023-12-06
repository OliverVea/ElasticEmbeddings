using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Providers;

public interface ITextEmbeddingProvider
{
    Task<Embedding> GetEmbeddingAsync(string text);
    Task<IReadOnlyList<Embedding>> GetEmbeddingsAsync(IReadOnlyList<string> texts);
}