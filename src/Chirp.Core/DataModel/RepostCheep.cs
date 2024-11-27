namespace Chirp.Core.DataModel;

public class RepostCheep : Cheep
{
    public required OriginalCheep Content { get; set; }
    public Author OriginalPoster { get { return Content.Author; } }
}