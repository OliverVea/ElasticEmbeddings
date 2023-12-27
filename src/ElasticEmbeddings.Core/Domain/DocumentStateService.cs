using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

public class DocumentStateService(IDocumentStateRepository documentStateRepository) : IDocumentStateService
{
    public Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken)
    {
        return documentStateRepository.SetDocumentStatesAsync(documentIds, state, cancellationToken);
    }

    public Task<long> GetDocumentCountWithStateAsync(DocumentState state, CancellationToken cancellationToken)
    {
        return documentStateRepository.GetDocumentCountWithStateAsync(state, cancellationToken);
    }

    public Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken)
    {
        return documentStateRepository.GetDocumentIdsWithStateAsync(state, maxElements, cancellationToken);
    }

    public Task SetAllDocumentStatesWithStateAsync(DocumentState from, DocumentState to, CancellationToken cancellationToken)
    {
        return documentStateRepository.SetAllDocumentStatesWithStateAsync(from, to, cancellationToken);
    }
}