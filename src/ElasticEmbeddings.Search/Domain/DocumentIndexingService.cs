using Elastic.Clients.Elasticsearch;
using ElasticEmbeddings.Search.Models;
using Microsoft.Extensions.Logging;

namespace ElasticEmbeddings.Search.Domain;

public class DocumentIndexingService(
    ILogger<DocumentIndexingService> logger,
    IDocumentIndexingQueue documentIndexingQueue,
    ElasticsearchClient elasticsearchClient,
    IDocumentMappingDescriptorService documentDocumentMappingDescriptorService) : IDocumentIndexingService
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var documentBatch = GetDocumentBatch();
        if (documentBatch.Count == 0) return;
        
        logger.LogInformation($"Indexing {documentBatch.Count} documents");

        await EnsureIndexAsync(cancellationToken);
        if (cancellationToken.IsCancellationRequested) return;
        
        await IndexBatchAsync(documentBatch, cancellationToken);
    }

    private List<Document> GetDocumentBatch()
    {
        var documents = new List<Document>();
        
        for (var i = 0; i < Constants.IngestionBatchSize; i++)
        {
            if (!documentIndexingQueue.Documents.TryDequeue(out var document)) break;
            documents.Add(document);
        }

        return documents;
    }

    private async Task EnsureIndexAsync(CancellationToken cancellationToken)
    {
        var indexExists = await elasticsearchClient.Indices.GetAsync(Constants.IndexName, cancellationToken);
        if (indexExists.IsValidResponse) return;
        
        var createIndexResponse = await elasticsearchClient.Indices.CreateAsync<Document>(Constants.IndexName, 
            configureRequest =>
        {
            configureRequest.Mappings(documentDocumentMappingDescriptorService.AddTypeMapping);
        }, cancellationToken);
        
        if (!createIndexResponse.IsValidResponse) throw new Exception(createIndexResponse.DebugInformation);
    }

    private async Task IndexBatchAsync(List<Document> documentBatch, CancellationToken cancellationToken)
    {
        var bulkResponse = await elasticsearchClient.BulkAsync(bulk => bulk
            .Index(Constants.IndexName)
            .IndexMany(documentBatch, (bulkDescriptor, document) => bulkDescriptor
                .Index(Constants.IndexName)
                .Id(document.DocumentId)), cancellationToken);

        if (!bulkResponse.IsValidResponse) throw new Exception(bulkResponse.DebugInformation);
    }
}