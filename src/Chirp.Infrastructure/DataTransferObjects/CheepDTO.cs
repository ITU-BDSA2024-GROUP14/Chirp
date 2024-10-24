using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.DataTransferObjects;

public class CheepDTO
{
    public string Text { get; set; }
    public string Timestamp { get; set; }
    public string Author { get; set; }

    public CheepDTO(Cheep cheep)
    {
        Text = cheep.Text;
        Timestamp = cheep.TimeStamp.ToString("MM/dd/yy H:mm:ss");
        Author = cheep.Author.Name;
    }
}