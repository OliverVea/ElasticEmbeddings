using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentEmbeddingService
{
    Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken);
    Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken);
}