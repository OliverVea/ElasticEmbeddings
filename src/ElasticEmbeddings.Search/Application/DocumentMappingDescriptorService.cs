using Elastic.Clients.Elasticsearch.Mapping;
using ElasticEmbeddings.Search.Interfaces;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Application;

public class DocumentMappingDescriptorService : IMappingDescriptorService<Document>
{
    public TypeMappingDescriptor<Document> AddTypeMapping(TypeMappingDescriptor<Document> typeMapping)
    {
        return typeMapping.Dynamic(DynamicMapping.Strict).Properties(AddMemoryDocumentPropertyMapping);
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