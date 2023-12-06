namespace ElasticEmbeddings.Search.Models;

public class Document
{
    public required string DocumentId { get; init; }
    public required float[] Embedding { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
}