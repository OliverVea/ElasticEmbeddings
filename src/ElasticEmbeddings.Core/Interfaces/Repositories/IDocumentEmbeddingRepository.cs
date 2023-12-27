using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Repositories;

public interface IDocumentEmbeddingRepository
{
    Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken);
    Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken);
}