namespace ElasticEmbeddings.API.Models;

public abstract class CreateDocumentRequest
{
    public required string Title { get; init; }
    public required string Text { get; init; }
}