using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Interfaces;

public interface IDocumentTextFormattingService
{
    string GetText(Document document);
}