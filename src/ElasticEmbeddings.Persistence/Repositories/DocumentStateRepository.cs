using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;
using ElasticEmbeddings.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticEmbeddings.Persistence.Repositories;

internal class DocumentStateRepository(IElasticEmbeddingsContext dbContext) : IDocumentStateRepository
{
    private readonly IElasticEmbeddingsContext _dbContext = dbContext;
    
    public async Task SetDocumentStatesAsync(IReadOnlyList<DocumentId> documentIds, DocumentState state, CancellationToken cancellationToken)
    {
        var documentIdGuids = documentIds.Select(x => x.Value).ToArray();
        
        var documentEntityIds = await _dbContext.Documents
            .Where(x => documentIdGuids.Contains(x.DocumentId))
            .Select(x => x.Id).ToArrayAsync(cancellationToken);

        var existingEntities = await _dbContext.DocumentStates
            .Where(x => documentIdGuids.Contains(x.Document.DocumentId))
            .ToArrayAsync(cancellationToken);

        foreach (var entity in existingEntities) entity.State = state;

        var newEntityIds = documentEntityIds.Except(existingEntities.Select(x => x.DocumentId)).ToArray();

        var entities = Map(newEntityIds, state);

        await _dbContext.DocumentStates.AddRangeAsync(entities, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<DocumentId>> GetDocumentIdsWithStateAsync(DocumentState state, int maxElements, CancellationToken cancellationToken)
    {
        return await _dbContext.DocumentStates
            .Where(x => x.State == state)
            .OrderBy(x => x.Id)
            .Take(maxElements)
            .Include(x => x.Document)
            .Select(x => new DocumentId(x.Document.DocumentId))
            .ToArrayAsync(cancellationToken);
    }

    public Task<long> GetDocumentCountWithStateAsync(DocumentState state, CancellationToken cancellationToken)
    {
        return _dbContext.DocumentStates
            .Where(x => x.State == state)
            .LongCountAsync(cancellationToken);
    }

    public async Task SetAllDocumentStatesWithStateAsync(DocumentState from, DocumentState to, CancellationToken cancellationToken)
    {
        await _dbContext.DocumentStates
            .Where(x => x.State == from)
            .ForEachAsync(x => x.State = to, cancellationToken);    
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    private static IEnumerable<DocumentStateEntity> Map(IEnumerable<long> documentEntityIds, DocumentState state)
    {
        return documentEntityIds.Select(x => Map(x, state));
    }

    private static DocumentStateEntity Map(long documentEntityId, DocumentState state)
    {
        return new DocumentStateEntity
        {
            State = state,
            DocumentId = documentEntityId
        };
    }
}