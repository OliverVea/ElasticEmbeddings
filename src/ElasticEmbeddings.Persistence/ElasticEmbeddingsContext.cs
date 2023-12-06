using ElasticEmbeddings.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticEmbeddings.Persistence;

internal class ElasticEmbeddingsContext(DbContextOptions<ElasticEmbeddingsContext> options) : DbContext(options), IElasticEmbeddingsContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentEntity>()
            .HasOne(x => x.DocumentEmbedding)
            .WithOne(x => x.Document);
        
        base.OnModelCreating(modelBuilder);
    }
    
    
    public required DbSet<DocumentEntity> Documents { get; init; }
    public required DbSet<DocumentEmbeddingEntity> DocumentEmbeddings { get; init; }
    public required DbSet<DocumentStateEntity> DocumentStates { get; init; }
}