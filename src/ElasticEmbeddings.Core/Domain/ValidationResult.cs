namespace ElasticEmbeddings.Domain;

public class ValidationResult
{
    public bool IsValid { get; private init; }
    public string? Message { get; private init; }
        
    private ValidationResult()
    {
            
    }
        
    public static ValidationResult Valid() => new()
    {
        IsValid = true
    };
    public static ValidationResult Invalid(string message) => new()
    {
        IsValid = false,
        Message = message
    };
}