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

    public Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken)
    {
        return documentStateRepository.GetDocumentIdsWithStateAsync(state, maxElements, cancellationToken);
    }
}