using ElasticEmbeddings.API.Mappers;
using ElasticEmbeddings.Embedding;
using ElasticEmbeddings.Persistence;
using ElasticEmbeddings.Search;
using ElasticEmbeddings.Search.Models;
using Microsoft.Data.Sqlite;

namespace ElasticEmbeddings.API;

public static class ServiceExtensions 
{
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");
        var sqliteConnection = new SqliteConnection(sqliteConnectionString);
        if (sqliteConnectionString is null) throw new InvalidOperationException("Sqlite connection string is not configured");
        
        var openAiConfiguration = configuration.GetSection("OpenAI").Get<OpenAIConfiguration>();
        if (openAiConfiguration is null) throw new InvalidOperationException("OpenAI configuration is not configured");
        
        var elasticConfiguration = configuration.GetSection("Elasticsearch").Get<ElasticsearchConfiguration>();
        if (elasticConfiguration is null) throw new InvalidOperationException("Elasticsearch configuration is not configured");
        
        services.AddCore();
        services.AddPersistence(sqliteConnection);
        services.AddSearch(elasticConfiguration);
        services.AddEmbedding(openAiConfiguration);

        services.AddScoped<IDocumentMapper, DocumentMapper>();
    }
    
}