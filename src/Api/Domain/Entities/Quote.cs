namespace Api.Domain.Entities;

public class Quote
{
    public int QuoteId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}