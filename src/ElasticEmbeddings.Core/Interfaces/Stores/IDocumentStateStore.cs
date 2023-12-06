using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Stores;

public interface IDocumentStateStore
{
    Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken);
}