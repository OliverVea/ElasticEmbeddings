namespace ElasticEmbeddings.Interfaces;

public interface IDocumentProcessingService
{
    Task ProcessDocumentBatchAsync(CancellationToken cancellationToken);
}