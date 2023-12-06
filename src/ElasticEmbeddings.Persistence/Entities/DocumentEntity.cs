using System.ComponentModel.DataAnnotations;

namespace ElasticEmbeddings.Persistence.Entities;

internal class DocumentEntity
{
    [Key]
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public long Id { get; set; }
    
    [Required]
    public required Guid DocumentId { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Title { get; init; }

    [Required]
    [StringLength(4096, MinimumLength = 1)]
    public required string Text { get; init; }
    
    public DocumentEmbeddingEntity? DocumentEmbedding { get; init; }
    public DocumentStateEntity? DocumentState { get; init; }
}