using ElasticEmbeddings.API.Models;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.API.Mappers;

public class DocumentMapper : IDocumentMapper
{
    public Document Map(CreateDocumentRequest request, Guid guid)
    {
        return new Document
        {
            DocumentId = new DocumentId(guid),
            Title = request.Title,
            Text = request.Text
        };
    }
}