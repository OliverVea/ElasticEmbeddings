using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Repositories;

public interface IEmbeddingSearchRepository
{
    Task IndexAsync(DocumentEmbedding documentEmbedding);
    Task<IEnumerable<DocumentResult>> SearchAsync(Embedding embedding);
    Task<long?> CountAsync();
}