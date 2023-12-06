namespace ElasticEmbeddings.Models;

public class DocumentEmbedding
{
    public required Document Document { get; init; }
    public required Embedding Embedding { get; init; }
}