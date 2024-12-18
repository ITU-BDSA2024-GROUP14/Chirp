using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;
/// <summary>
/// This DataTransferObject represents a Cheep without the fields we don't need in
/// the rest of the program from the Cheep, OriginalCheep or RepostedCheep classes.
/// </summary>
public class CheepDTO
{
    public string Text { get; set; }
    public string Timestamp { get; set; }
    public string Author { get; set; }
    public int Id { get; set; }
    public string? ReCheepedAuthor { get; set; }
    /// <summary>
    /// This constructor creates a CheepDTO from a Cheep object.
    /// </summary>
    /// <param name="cheep">The Cheep object from which to instantiate a CheepDTO</param>
    public CheepDTO(Cheep cheep)
    {
        Text = cheep.GetText();
        Timestamp = cheep.TimeStamp.ToString("dd/MM/yy H:mm:ss");
        Author = cheep.Author.DisplayName;
        Id = cheep.CheepId;

        if (cheep is RepostCheep recheep)
        {
            ReCheepedAuthor = recheep.OriginalPoster.DisplayName;
        }
    }
}