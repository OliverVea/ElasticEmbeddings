using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Application;

internal class DocumentEmbeddingGeneratorService(ITextEmbeddingProvider textEmbeddingProvider) : IDocumentEmbeddingGeneratorService
{
    private const string TextTemplate = "# {0}\n{1}";

    public async Task<IReadOnlyList<DocumentEmbedding>> GetEmbeddingsAsync(IReadOnlyList<Document> documents,
        CancellationToken cancellationToken)
    {
        if (!documents.Any()) return Array.Empty<DocumentEmbedding>();
        
        var documentTexts = documents.Select(GetDocumentText).ToArray();
        
        var embeddings = await textEmbeddingProvider.GetEmbeddingsAsync(documentTexts);
        
        return documents.Zip(embeddings).Select(documentEmbeddingPair => new DocumentEmbedding
        {
            Document = documentEmbeddingPair.First,
            Embedding = documentEmbeddingPair.Second
        }).ToArray();
    }

    private string GetDocumentText(Document document)
    {
        return string.Format(TextTemplate, document.Title, document.Text);
    }
}