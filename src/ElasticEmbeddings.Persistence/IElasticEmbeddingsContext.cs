using ElasticEmbeddings.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticEmbeddings.Persistence;

internal interface IElasticEmbeddingsContext
{
    DbSet<DocumentEntity> Documents { get; }
    DbSet<DocumentEmbeddingEntity> DocumentEmbeddings { get; }
    DbSet<DocumentStateEntity> DocumentStates { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}