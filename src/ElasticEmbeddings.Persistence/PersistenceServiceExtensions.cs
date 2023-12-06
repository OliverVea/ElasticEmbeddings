using ElasticEmbeddings.Interfaces.Stores;
using ElasticEmbeddings.Persistence.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings.Persistence;

public static class PersistenceServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        AddDbContext(services, connectionString);

        services.AddScoped<IDocumentStore, DocumentStore>();
        services.AddScoped<IDocumentEmbeddingStore, DocumentEmbeddingStore>();
        services.AddScoped<IDocumentStateStore, DocumentStateStore>();
    }

    private static void AddDbContext(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ElasticEmbeddingsContext>(options =>
        {
            options.UseSqlite(connectionString);
            
            options.ConfigureWarnings(w =>
            {
                w.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning);
                w.Throw(RelationalEventId.BoolWithDefaultWarning);
                w.Throw(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                w.Throw(CoreEventId.FirstWithoutOrderByAndFilterWarning);
                w.Throw(CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning);
            });
        });

        services.AddScoped<IElasticEmbeddingsContext>(provider => provider.GetRequiredService<ElasticEmbeddingsContext>());
    }
}