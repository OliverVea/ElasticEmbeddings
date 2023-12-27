using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

public class DocumentEmbeddingService(IDocumentEmbeddingRepository documentEmbeddingRepository, IDocumentStateService documentStateService) : IDocumentEmbeddingService
{
    public async Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken)
    {
        await documentEmbeddingRepository.SetAsync(documentEmbeddings, cancellationToken);

        var documentIdsWithEmbeddings = documentEmbeddings.Select(x => x.Document.DocumentId).ToArray();
        
        await documentStateService.SetDocumentStatesAsync(documentIdsWithEmbeddings, DocumentState.Embedded, cancellationToken);
    }

    public Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        return documentEmbeddingRepository.GetAsync(documentIds, cancellationToken);
    }
}