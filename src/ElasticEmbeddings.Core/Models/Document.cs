namespace ElasticEmbeddings.Models;

public class Document
{
    public required DocumentId DocumentId { get; init; }
    public required string Title { get; init; }
    public string Text { get; init; } = string.Empty;
}