namespace ElasticEmbeddings.Models;

public class DocumentSearchResult(IEnumerable<DocumentResult> documentResults)
{
    public IEnumerable<DocumentResult> Documents { get; } = documentResults;
}