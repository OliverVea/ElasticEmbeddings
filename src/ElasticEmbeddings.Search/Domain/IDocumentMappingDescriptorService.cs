using Elastic.Clients.Elasticsearch.Mapping;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Domain;

public interface IDocumentMappingDescriptorService
{
    public void AddTypeMapping(TypeMappingDescriptor<Document> descriptor);
}