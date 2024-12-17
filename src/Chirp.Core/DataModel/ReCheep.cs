namespace Chirp.Core.DataModel;
/// <summary>
/// Represents a reposted cheep from any author.
/// </summary>
public class RepostCheep : Cheep
{
    public required OriginalCheep Content { get; init; }
    public Author OriginalPoster { get { return Content.Author; } }
    
    /// <returns>The text of the Content (original cheep)</returns>
    public override string GetText()
    {
        return Content.GetText();
    }
}