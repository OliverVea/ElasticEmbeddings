using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentEmbeddingGeneratorService
{
    Task<IReadOnlyList<DocumentEmbedding>> GetEmbeddingsAsync(IReadOnlyList<Document> document,
        CancellationToken cancellationToken);
}