using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Models;
using Microsoft.Extensions.Logging;

namespace ElasticEmbeddings.Application;

internal class DocumentProcessingService(
    IDocumentStateService documentStateService,
    IDocumentService documentService,
    IDocumentEmbeddingGeneratorService documentEmbeddingGeneratorService,
    IDocumentIndexingService documentIndexingService,
    IDocumentEmbeddingService documentEmbeddingService) : IDocumentProcessingService
{
    private const int EmbeddingBatchSize = 100;
    private const int IndexingBatchSize = 1000;
    
    public async Task ProcessDocumentBatchAsync(CancellationToken cancellationToken)
    {
        await ProcessDocumentEmbeddingBatchAsync(cancellationToken);
        await ProcessDocumentIndexingBatchAsync(cancellationToken);
    }

    private async Task ProcessDocumentEmbeddingBatchAsync(CancellationToken cancellationToken)
    {
        var updatedDocumentIdBatch =
            await GetDocumentIdsWithStateAsync(DocumentState.Updated, EmbeddingBatchSize, cancellationToken);
        if (!updatedDocumentIdBatch.Any()) return;

        var documents = await documentService.GetAsync(updatedDocumentIdBatch, cancellationToken);
        
        var documentEmbeddings = await documentEmbeddingGeneratorService.GetEmbeddingsAsync(documents, cancellationToken);
        
        await documentEmbeddingService.SetAsync(documentEmbeddings, cancellationToken);
    }

    private async Task ProcessDocumentIndexingBatchAsync(CancellationToken cancellationToken)
    {
        var embeddedDocumentIdBatch =
            await GetDocumentIdsWithStateAsync(DocumentState.Embedded, IndexingBatchSize, cancellationToken);
        if (!embeddedDocumentIdBatch.Any()) return;
            
        var documentEmbeddings = await documentEmbeddingService.GetAsync(embeddedDocumentIdBatch, cancellationToken);
        if (!documentEmbeddings.Any())
        {
            throw new ApplicationException("Found embedded document ids but no documents could be retrieved for the ids.");
        }
        
        await documentIndexingService.IndexAsync(documentEmbeddings, cancellationToken);
    }

    private async Task<IReadOnlyList<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken)
    {
        var documentIds = 
            await documentStateService.GetDocumentIdsWithStateAsync(state, maxElements, cancellationToken);
        
        return documentIds as IReadOnlyList<DocumentId> ?? documentIds.ToArray();
        
    }
}