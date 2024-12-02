using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;

public class CheepDTO
{
    public string Text { get; set; }
    public string Timestamp { get; set; }
    public string Author { get; set; }
    public int Id { get; set; }
    public string? ReCheepedAuthor { get; set; }

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