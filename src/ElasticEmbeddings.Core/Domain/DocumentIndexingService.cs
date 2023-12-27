using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

public class DocumentIndexingService(IEmbeddingSearchRepository embeddingSearchRepository, IDocumentStateService documentStateService) : IDocumentIndexingService
{
    public async Task IndexAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken)
    {
        var indexingTasks = documentEmbeddings.Select(embeddingSearchRepository.IndexAsync);
        await Task.WhenAll(indexingTasks);

        var documentIdsWithEmbeddings = documentEmbeddings.Select(x => x.Document.DocumentId).ToArray();

        await documentStateService.SetDocumentStatesAsync(documentIdsWithEmbeddings, DocumentState.Indexed, cancellationToken);
    }
}