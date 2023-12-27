using Elastic.Clients.Elasticsearch.Mapping;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Domain;

public class DocumentMappingDescriptorService : IDocumentMappingDescriptorService
{
    public void AddTypeMapping(TypeMappingDescriptor<Document> descriptor)
    {
        descriptor.Dynamic(DynamicMapping.Strict).Properties(AddMemoryDocumentPropertyMapping);
    }

    private static void AddMemoryDocumentPropertyMapping(PropertiesDescriptor<Document> properties)
    {
        properties
            .Keyword(doc => doc.DocumentId)
            .DenseVector(doc => doc.Embedding, opts => opts.Index().Similarity(Constants.Similarity).Dims(Constants.EmbeddingDimensions))
            .Keyword(doc => doc.Title, k => k.Index(false).DocValues(false))
            .Keyword(doc => doc.Text, k => k.Index(false).DocValues(false));
    }
}