using ElasticEmbeddings.Domain.DocumentTextFormatters;
using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain;

public class DocumentTextFormattingService : IDocumentTextFormattingService
{
    public string GetText(Document document)
    {
        var formatter = new YamlFormatter
        {
            Document = document
        };

        return formatter.TransformText();
    }
}