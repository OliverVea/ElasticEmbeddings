using ElasticEmbeddings.Domain;
using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Jobs;
using ElasticEmbeddings.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings;

public static class CoreServiceExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IDocumentEmbeddingGeneratorService, DocumentEmbeddingGeneratorService>();
        services.AddScoped<IDocumentEmbeddingService, DocumentEmbeddingService>();
        services.AddScoped<IDocumentIndexingService, DocumentIndexingService>();
        services.AddScoped<IDocumentProcessingService, DocumentProcessingService>();
        services.AddScoped<IDocumentSearchService, DocumentSearchService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDocumentStateService, DocumentStateService>();
        services.AddScoped<IQueryEmbeddingGeneratorService, QueryEmbeddingGeneratorService>();
        services.AddScoped<IDocumentTextFormattingService, DocumentTextFormattingService>();

        services.AddHostedService<DocumentProcessingJob>();
    }
}