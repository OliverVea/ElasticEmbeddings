using ElasticEmbeddings.Interfaces.Stores;
using ElasticEmbeddings.Models;
using ElasticEmbeddings.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticEmbeddings.Persistence.Stores;

internal class DocumentStore(IElasticEmbeddingsContext dbContext) : IDocumentStore
{
    public async Task StoreAsync(Document document, CancellationToken cancellationToken)
    {
        await DeleteAsync(document.DocumentId, cancellationToken);
        
        var documentEntity = Map(document);
        
        await dbContext.Documents.AddAsync(documentEntity, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Document?> GetAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return dbContext.Documents
            .Where(x => x.DocumentId == documentId.Value)
            .Select(x => Map(x))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> GetAsync(IReadOnlyList<DocumentId> documentIds, CancellationToken cancellationToken)
    {
        var documentIdGuids = documentIds.Select(x => x.Value).ToHashSet();
        
        return await dbContext.Documents
            .Where(x => documentIdGuids.Contains(x.DocumentId))
            .Select(x => Map(x))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DocumentId>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Documents
            .Select(x => new DocumentId(x.DocumentId))
            .ToArrayAsync(cancellationToken);
    }

    public Task DeleteAsync(DocumentId documentId, CancellationToken cancellationToken)
    {
        return dbContext.Documents
            .Where(x => x.DocumentId == documentId.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    # region Map

    private static DocumentEntity Map(Document document)
    {
        return new DocumentEntity
        {
            DocumentId = document.DocumentId.Value,
            Title = document.Title,
            Text = document.Text
        };
    }
    
    private static Document Map(DocumentEntity entity)
    {
        return new Document
        {
            DocumentId = new DocumentId(entity.DocumentId),
            Title = entity.Title,
            Text = entity.Text
        };
    }
    
    #endregion
}