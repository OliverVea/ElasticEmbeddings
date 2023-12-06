using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Stores;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

internal class DocumentService(IDocumentStore documentStore, IDocumentStateService documentStateService) : IDocumentService
{
    public async Task SetAsync(Document document, CancellationToken cancellationToken)
    {
        await documentStore.StoreAsync(document, cancellationToken);

        var documentIds = new[] { document.DocumentId };
        await documentStateService.SetDocumentStatesAsync(documentIds, DocumentState.Updated, cancellationToken);
    }

    public Task<Document?> GetAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return documentStore.GetAsync(documentId, cancellationToken);
    }

    public Task<IReadOnlyList<Document>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        return documentStore.GetAsync(documentIds, cancellationToken);
    }

    public Task<IReadOnlyList<DocumentId>> ListIdsAsync(CancellationToken cancellationToken)
    {
        return documentStore.GetAllAsync(cancellationToken);
    }

    public Task DeleteAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return documentStore.DeleteAsync(documentId, cancellationToken);
    }
}