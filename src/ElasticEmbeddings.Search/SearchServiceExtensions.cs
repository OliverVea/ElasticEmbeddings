using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Search.Application;
using ElasticEmbeddings.Search.Interfaces;
using ElasticEmbeddings.Search.Jobs;
using ElasticEmbeddings.Search.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings.Search;

public static class SearchServiceExtensions
{
    public static void AddSearch(this IServiceCollection services, ElasticsearchConfiguration configuration)
    {
        AddElasticsearch(services, configuration);
        
        services.AddScoped<IEmbeddingSearchProvider, EmbeddingSearchProvider>();
        services.AddScoped<IDocumentIndexingService, DocumentIndexingService>();
        services.AddScoped<IMappingDescriptorService<Document>, DocumentMappingDescriptorService>();
        
        services.AddSingleton<IDocumentMapper, DocumentMapper>();
        services.AddSingleton<IElasticIndexState, ElasticIndexState>();
        
        services.AddHostedService<ElasticIndexingJob>();
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