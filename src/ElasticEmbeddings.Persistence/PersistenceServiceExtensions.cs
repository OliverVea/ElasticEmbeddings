using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Persistence.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings.Persistence;

public static class PersistenceServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, SqliteConnection sqliteConnection)
    {
        AddSqliteDbContext(services, sqliteConnection);
    
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IDocumentEmbeddingRepository, DocumentEmbeddingRepository>();
        services.AddScoped<IDocumentStateRepository, DocumentStateRepository>();
    }

    private static void AddSqliteDbContext(IServiceCollection services, SqliteConnection sqliteConnection)
    {
        services.AddDbContext<ElasticEmbeddingsContext>(options =>
        {
            options.UseSqlite(sqliteConnection);
            
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
    
    public static async Task EnsureCreatedAsync(this IServiceProvider serviceProvider, SqliteConnection sqliteConnection)
    {
        await sqliteConnection.OpenAsync();
        
        var dbContext = serviceProvider.GetRequiredService<ElasticEmbeddingsContext>();
        
        await dbContext.Database.EnsureCreatedAsync();
    }
}