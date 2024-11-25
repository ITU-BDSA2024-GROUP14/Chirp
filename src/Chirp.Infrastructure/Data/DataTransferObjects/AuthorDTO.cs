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
        if (author.Cheeps != null)
        {
            Cheeps = (List<CheepDTO>)author.Cheeps.Select(cheep => new CheepDTO(cheep));
        }
        Following = (List<string>)author.Following.Select(a => a.Beak);
    }
}