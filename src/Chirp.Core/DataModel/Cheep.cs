namespace Chirp.Core.DataModel;

public class Cheep
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
}