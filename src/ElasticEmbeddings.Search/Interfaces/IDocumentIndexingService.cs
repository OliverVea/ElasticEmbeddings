namespace ElasticEmbeddings.Search.Interfaces;

public interface IDocumentIndexingService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}