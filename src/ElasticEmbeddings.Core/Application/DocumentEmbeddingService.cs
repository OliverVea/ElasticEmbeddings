using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Stores;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

public class DocumentEmbeddingService(IDocumentEmbeddingStore documentEmbeddingStore, IDocumentStateService documentStateService) : IDocumentEmbeddingService
{
    public async Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken)
    {
        await documentEmbeddingStore.SetAsync(documentEmbeddings, cancellationToken);

        var documentIdsWithEmbeddings = documentEmbeddings.Select(x => x.Document.DocumentId).ToArray();
        
        await documentStateService.SetDocumentStatesAsync(documentIdsWithEmbeddings, DocumentState.Embedded, cancellationToken);
    }

    public Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        return documentEmbeddingStore.GetAsync(documentIds, cancellationToken);
    }
}