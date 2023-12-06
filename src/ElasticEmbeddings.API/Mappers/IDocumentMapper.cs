using ElasticEmbeddings.API.Models;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.API.Mappers;

public interface IDocumentMapper
{
    Document Map(CreateDocumentRequest request, Guid guid);
}