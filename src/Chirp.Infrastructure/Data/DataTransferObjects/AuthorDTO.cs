using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;

public class AuthorDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<CheepDTO> Cheeps { get; set; }

    public List<string> Following { get; set; }

    public AuthorDTO(Author author)
    {
        Name = author.Beak;
        Email = author.Email;
        Cheeps = [];
        if (author.Cheeps != null)
        {
            foreach (var cheep in author.Cheeps)
            {
                Cheeps.Add(new CheepDTO(cheep));
            }
        }

        Following = [];
        foreach (var a in author.Following)
        {
            Following.Add(a.Beak);
        }
    }
}