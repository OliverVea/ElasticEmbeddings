using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;
using ElasticEmbeddings.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticEmbeddings.Persistence.Repositories;

internal class DocumentEmbeddingRepository(IElasticEmbeddingsContext dbContext) : IDocumentEmbeddingRepository
{
    public async Task SetAsync(IReadOnlyList<DocumentEmbedding> documentEmbeddings, CancellationToken cancellationToken)
    {

        var documentIdToEntityIdLookup =
            await dbContext.Documents.ToDictionaryAsync(x => x.DocumentId, x => x.Id, cancellationToken);

        if (documentEmbeddings.Any(x => !documentIdToEntityIdLookup.ContainsKey(x.Document.DocumentId.Value)))
            throw new ArgumentException("Could not find some documents in the db");
        
        var documentIds = documentEmbeddings.Select(x => x.Document.DocumentId.Value).ToArray();
        
        await dbContext.DocumentEmbeddings
            .Where(x => documentIds.Contains(x.Document.DocumentId))
            .ExecuteDeleteAsync(cancellationToken);

        var entities = Map(documentEmbeddings, documentIdToEntityIdLookup);

        await dbContext.DocumentEmbeddings.AddRangeAsync(entities, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DocumentEmbedding>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        var documentGuids = documentIds.Select(x => x.Value).ToArray();
        var documentEmbeddings = await dbContext.DocumentEmbeddings
            .Include(x => x.Document)
            .Where(x => documentGuids.Contains(x.Document.DocumentId))
            .ToArrayAsync(cancellationToken);

        return Map(documentEmbeddings);
    }

    
    
    private static IEnumerable<DocumentEmbeddingEntity> Map(IEnumerable<DocumentEmbedding> documentEmbeddings,
        Dictionary<Guid, long> documentIdToEntityIdLookup)
    {
        return documentEmbeddings
            .Select(x => Map(x, documentIdToEntityIdLookup))
            .Where(x => x is not null)!;
    }

    private static DocumentEmbeddingEntity? Map(DocumentEmbedding documentEmbedding,
        Dictionary<Guid, long> documentIdToEntityIdLookup)
    {
        var documentId = documentEmbedding.Document.DocumentId.Value;
        if (!documentIdToEntityIdLookup.TryGetValue(documentId, out var documentEntity)) return null;
        
        return new DocumentEmbeddingEntity
        {
            Embeddings = documentEmbedding.Embedding.Embeddings.ToArray(),
            DocumentId = documentEntity
        };
    }

    private static DocumentEmbedding[] Map(IEnumerable<DocumentEmbeddingEntity> entities)
    {
        return entities.Select(Map).ToArray();
    }

    private static DocumentEmbedding Map(DocumentEmbeddingEntity entity)
    {
        var document = new Document
        {
            DocumentId = new DocumentId(entity.Document.DocumentId),
            Title = entity.Document.Title,
            Text = entity.Document.Text
        };

        var embedding = new Models.Embedding
        {
            Embeddings = entity.Embeddings
        };
        
        return new DocumentEmbedding
        {
            Document = document,
            Embedding = embedding
        };
    }
}