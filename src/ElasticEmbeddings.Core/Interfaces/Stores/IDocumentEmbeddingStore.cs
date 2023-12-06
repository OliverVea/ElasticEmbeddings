using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Stores;

public interface IDocumentEmbeddingStore
{
    Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken);
    Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken);
}