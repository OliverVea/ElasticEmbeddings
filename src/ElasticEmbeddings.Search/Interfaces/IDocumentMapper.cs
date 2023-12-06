using Elastic.Clients.Elasticsearch.Core.Search;
using ElasticEmbeddings.Models;
using Document = ElasticEmbeddings.Search.Models.Document;

namespace ElasticEmbeddings.Search.Interfaces;

public interface IDocumentMapper
{
    Document Map(DocumentEmbedding documentEmbedding);
    IEnumerable<DocumentResult> Map(IReadOnlyCollection<Hit<Document>> documentHits);
}