using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Models;
using Microsoft.Extensions.Logging;

namespace ElasticEmbeddings.Domain;

internal class DocumentProcessingService(
    IDocumentStateService documentStateService,
    IDocumentService documentService,
    IDocumentEmbeddingGeneratorService documentEmbeddingGeneratorService,
    IDocumentIndexingService documentIndexingService,
    IDocumentEmbeddingService documentEmbeddingService,
    ILogger<DocumentProcessingService> logger) : IDocumentProcessingService
{
    private const int EmbeddingBatchSize = 100;
    private const int IndexingBatchSize = 1000;
    private const decimal IndexedDocumentTolerance = 0.01m;
    
    public async Task ProcessDocumentBatchAsync(CancellationToken cancellationToken)
    {
        await EnsureDocumentStateIsConsistentAsync(cancellationToken);
        
        await ProcessDocumentEmbeddingBatchAsync(cancellationToken);
        await ProcessDocumentIndexingBatchAsync(cancellationToken);
    }

    private async Task EnsureDocumentStateIsConsistentAsync(CancellationToken cancellationToken)
    {
        var result = await IndexedDocumentsAreInIndex(cancellationToken);
        if (result.IsValid) return;

        logger.LogInformation(result.Message);
        logger.LogInformation("Resetting document states of indexed documents to embedded");

        await documentStateService.SetAllDocumentStatesWithStateAsync(DocumentState.Indexed, DocumentState.Embedded, cancellationToken);
    }

    private async Task<ValidationResult> IndexedDocumentsAreInIndex(CancellationToken cancellationToken)
    {
        var documentsWithIndexedState = await documentStateService.GetDocumentCountWithStateAsync(DocumentState.Indexed, cancellationToken);
        if (documentsWithIndexedState == 0) return ValidationResult.Valid();
        
        var indexedDocumentCount = await documentIndexingService.GetIndexedDocumentCount();

        switch (indexedDocumentCount)
        {
            case null:
                return ValidationResult.Invalid($"Found [{documentsWithIndexedState}] documents with state [{DocumentState.Indexed}] but index does not exist");
            case 0:
                return ValidationResult.Invalid($"Found [{documentsWithIndexedState}] documents with state [{DocumentState.Indexed}] but index contains no documents");
        }

        var indexedDocumentRatio = documentsWithIndexedState / (decimal)indexedDocumentCount;
        var indexedDocumentRatioValid = indexedDocumentRatio is > 1 - IndexedDocumentTolerance and < 1 + IndexedDocumentTolerance;

        if (!indexedDocumentRatioValid)
        {
            return ValidationResult.Invalid($"Found [{documentsWithIndexedState}] documents with state [{DocumentState.Indexed}] but index contains [{indexedDocumentCount}] documents");
        }
        
        return ValidationResult.Valid();
    }

    private async Task ProcessDocumentEmbeddingBatchAsync(CancellationToken cancellationToken)
    {
        var updatedDocumentIdBatch =
            await GetDocumentIdsWithStateAsync(DocumentState.Updated, EmbeddingBatchSize, cancellationToken);
        if (!updatedDocumentIdBatch.Any()) return;
        
        logger.LogInformation($"Generating embeddings for [{updatedDocumentIdBatch.Count}] documents");

        var documents = await documentService.GetAsync(updatedDocumentIdBatch, cancellationToken);
        
        var documentEmbeddings = await documentEmbeddingGeneratorService.GetEmbeddingsAsync(documents, cancellationToken);
        
        await documentEmbeddingService.SetAsync(documentEmbeddings, cancellationToken);
    }

    private async Task ProcessDocumentIndexingBatchAsync(CancellationToken cancellationToken)
    {
        var embeddedDocumentIdBatch =
            await GetDocumentIdsWithStateAsync(DocumentState.Embedded, IndexingBatchSize, cancellationToken);
        if (!embeddedDocumentIdBatch.Any()) return;
        
        logger.LogInformation($"Enqueueing [{embeddedDocumentIdBatch.Count}] documents for indexing");
            
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