namespace ElasticEmbeddings.Interfaces.Repositories;

public interface ITextEmbeddingRepository
{
    Task<Models.Embedding> GetEmbeddingAsync(string text);
    Task<IReadOnlyList<Models.Embedding>> GetEmbeddingsAsync(IReadOnlyList<string> texts);
}