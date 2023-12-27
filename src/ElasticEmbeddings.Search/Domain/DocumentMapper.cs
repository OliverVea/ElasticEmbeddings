using Elastic.Clients.Elasticsearch.Core.Search;
using ElasticEmbeddings.Models;
using Document = ElasticEmbeddings.Search.Models.Document;

namespace ElasticEmbeddings.Search.Domain;

public class DocumentMapper : IDocumentMapper
{
    public Document Map(DocumentEmbedding documentEmbedding)
    {
        return new Document
        {
            DocumentId = documentEmbedding.Document.DocumentId.Value.ToString(),
            Title = documentEmbedding.Document.Title,
            Text = documentEmbedding.Document.Text,
            Embedding = documentEmbedding.Embedding.Embeddings.ToArray()
        };
    }

    public IEnumerable<DocumentResult> Map(IReadOnlyCollection<Hit<Document>> documentHits)
    {
        return documentHits
            .Where(x => x.Score is not null && x.Source is not null)
            .Select(x => Map(x.Source!, x.Score!.Value));
    }

    private static DocumentResult Map(Document hitDocument, double score)
    {
        var documentIdGuid = Guid.Parse(hitDocument.DocumentId);
        if (documentIdGuid == Guid.Empty) throw new Exception($"DocumentId {hitDocument.DocumentId} is not a valid Guid");
        
        
        var document = new ElasticEmbeddings.Models.Document
        {
            DocumentId = new DocumentId(documentIdGuid),
            Title = hitDocument.Title,
            Text = hitDocument.Text
        };
        
        return new DocumentResult(document, (float)score);
    }
}