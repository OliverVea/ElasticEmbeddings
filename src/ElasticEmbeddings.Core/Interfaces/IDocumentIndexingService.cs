using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentIndexingService
{
    Task IndexAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken);
    Task<long?> GetIndexedDocumentCount();
}