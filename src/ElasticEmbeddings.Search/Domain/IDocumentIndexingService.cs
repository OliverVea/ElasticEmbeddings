namespace ElasticEmbeddings.Search.Domain;

public interface IDocumentIndexingService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}