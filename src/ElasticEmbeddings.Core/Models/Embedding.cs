namespace ElasticEmbeddings.Models;

public readonly record struct Embedding
{
    public required ReadOnlyMemory<float> Embeddings { get; init; }
};