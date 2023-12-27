using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

internal class DocumentEmbeddingGeneratorService(
    IDocumentTextFormattingService documentTextFormattingService, 
    ITextEmbeddingRepository textEmbeddingRepository) : IDocumentEmbeddingGeneratorService
{
    public async Task<IReadOnlyList<DocumentEmbedding>> GetEmbeddingsAsync(IReadOnlyList<Document> documents,
        CancellationToken cancellationToken)
    {
        if (!documents.Any()) return Array.Empty<DocumentEmbedding>();
        
        var documentTexts = documents.Select(documentTextFormattingService.GetText).ToArray();
        
        var embeddings = await textEmbeddingRepository.GetEmbeddingsAsync(documentTexts);
        
        return documents.Zip(embeddings).Select(documentEmbeddingPair => new DocumentEmbedding
        {
            Document = documentEmbeddingPair.First,
            Embedding = documentEmbeddingPair.Second
        }).ToArray();
    }
}