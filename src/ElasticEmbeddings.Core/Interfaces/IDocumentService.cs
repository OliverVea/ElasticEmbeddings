using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentService
{
    Task SetAsync(Document document, CancellationToken cancellationToken);
    Task<Document?> GetAsync(DocumentId documentId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Document>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken);
    Task<IReadOnlyList<DocumentId>> ListIdsAsync(CancellationToken cancellationToken);
    Task DeleteAsync(DocumentId documentId, CancellationToken cancellationToken);
}