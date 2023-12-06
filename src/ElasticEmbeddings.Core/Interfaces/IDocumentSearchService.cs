using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentSearchService
{
    Task<DocumentSearchResult> SearchAsync(DocumentSearchRequest request);
}