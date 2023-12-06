using Elastic.Clients.Elasticsearch.Mapping;

namespace ElasticEmbeddings.Search.Interfaces;

public interface IMappingDescriptorService<T> where T : class
{
    public TypeMappingDescriptor<T> AddTypeMapping(TypeMappingDescriptor<T> descriptor);
}