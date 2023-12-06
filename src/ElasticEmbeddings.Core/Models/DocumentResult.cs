namespace ElasticEmbeddings.Models;

public class DocumentResult(Document document, float score)
{
    public Document Document { get; } = document;
    public float Score { get; } = score;
}