using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Providers;

public interface IEmbeddingSearchProvider
{
    Task IndexAsync(DocumentEmbedding documentEmbedding);
    Task<IEnumerable<DocumentResult>> SearchAsync(Embedding embedding);
}