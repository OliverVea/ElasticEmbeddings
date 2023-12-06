using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentStateService
{
    Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken);
}