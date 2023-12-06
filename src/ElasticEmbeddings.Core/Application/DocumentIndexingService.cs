using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

public class DocumentIndexingService(IEmbeddingSearchProvider embeddingSearchProvider, IDocumentStateService documentStateService) : IDocumentIndexingService
{
    public async Task IndexAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken)
    {
        var indexingTasks = documentEmbeddings.Select(embeddingSearchProvider.IndexAsync);
        await Task.WhenAll(indexingTasks);

        var documentIdsWithEmbeddings = documentEmbeddings.Select(x => x.Document.DocumentId).ToArray();

        await documentStateService.SetDocumentStatesAsync(documentIdsWithEmbeddings, DocumentState.Indexed, cancellationToken);
    }
}