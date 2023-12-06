namespace ElasticEmbeddings.Interfaces;

internal interface IDocumentProcessingService
{
    Task ProcessDocumentBatchAsync(CancellationToken cancellationToken);
}