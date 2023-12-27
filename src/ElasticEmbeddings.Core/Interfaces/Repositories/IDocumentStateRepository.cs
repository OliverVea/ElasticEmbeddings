using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces.Repositories;

public interface IDocumentStateRepository
{
    Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken);
    Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken);
    Task<long> GetDocumentCountWithStateAsync(DocumentState state, CancellationToken cancellationToken);
    Task SetAllDocumentStatesWithStateAsync(DocumentState from, DocumentState to, CancellationToken cancellationToken);
}