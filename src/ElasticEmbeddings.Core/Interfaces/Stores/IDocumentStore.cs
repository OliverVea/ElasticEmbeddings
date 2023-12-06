using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Stores;

public interface IDocumentStore
{
    Task StoreAsync(Document document, CancellationToken cancellationToken);
    Task<Document?> GetAsync(DocumentId documentId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Document>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken);
    Task<IReadOnlyList<DocumentId>> GetAllAsync(CancellationToken cancellationToken);
    Task DeleteAsync(DocumentId documentId, CancellationToken cancellationToken);
}