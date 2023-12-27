using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Search.Domain;
using ElasticEmbeddings.Search.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings.Search;

public static class SearchServiceExtensions
{
    public static void AddSearch(this IServiceCollection services, ElasticsearchConfiguration configuration)
    {
        AddElasticsearch(services, configuration);
        
        services.AddScoped<IEmbeddingSearchRepository, EmbeddingSearchRepository>();
        services.AddScoped<IDocumentIndexingService, DocumentIndexingService>();
        services.AddScoped<IDocumentMappingDescriptorService, DocumentMappingDescriptorService>();
        
        services.AddSingleton<IDocumentMapper, DocumentMapper>();
        services.AddSingleton<IDocumentIndexingQueue, DocumentIndexingQueue>();
        
        services.AddHostedService<DocumentIndexingJob>();
    }

    private static void AddElasticsearch(IServiceCollection services, ElasticsearchConfiguration configuration)
    {
        var endpoint = new Uri(configuration.Endpoint);

        var settings = new ElasticsearchClientSettings(endpoint).EnableDebugMode()
            .IgnoreCertificateValidation()
            .DisableDirectStreaming()
            .Authentication(new BasicAuthentication(configuration.Username, configuration.Password));

        var client = new ElasticsearchClient(settings);
        
        services.AddSingleton(client);
    }
    
    private static ElasticsearchClientSettings IgnoreCertificateValidation(this ElasticsearchClientSettings settings)
    {
        return settings.ServerCertificateValidationCallback((_, _, _, _) => true);
    }
}