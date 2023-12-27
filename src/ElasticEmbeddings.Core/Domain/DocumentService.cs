using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

internal class DocumentService(IDocumentRepository documentRepository, IDocumentStateService documentStateService) : IDocumentService
{
    public async Task SetAsync(Document document, CancellationToken cancellationToken)
    {
        await documentRepository.StoreAsync(document, cancellationToken);

        var documentIds = new[] { document.DocumentId };
        await documentStateService.SetDocumentStatesAsync(documentIds, DocumentState.Updated, cancellationToken);
    }

    public Task<Document?> GetAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return documentRepository.GetAsync(documentId, cancellationToken);
    }

    public Task<IReadOnlyList<Document>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        return documentRepository.GetAsync(documentIds, cancellationToken);
    }

    public Task<IReadOnlyList<DocumentId>> ListIdsAsync(CancellationToken cancellationToken)
    {
        return documentRepository.GetAllAsync(cancellationToken);
    }

    public Task DeleteAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return documentRepository.DeleteAsync(documentId, cancellationToken);
    }
}