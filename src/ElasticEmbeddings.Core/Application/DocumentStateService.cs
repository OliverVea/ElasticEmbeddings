using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Stores;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

public class DocumentStateService(IDocumentStateStore documentStateStore) : IDocumentStateService
{
    public Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken)
    {
        return documentStateStore.SetDocumentStatesAsync(documentIds, state, cancellationToken);
    }

    public Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken)
    {
        return documentStateStore.GetDocumentIdsWithStateAsync(state, maxElements, cancellationToken);
    }
}