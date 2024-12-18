using System.Diagnostics.CodeAnalysis;
using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;

public class AuthorDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }

    public required List<string> Following { get; set; }

    [SetsRequiredMembers]
    public AuthorDTO(Author author)
    {
        Name = author.DisplayName;
        Email = author.Email;
        Cheeps = [];

        foreach (var cheep in author.Cheeps)
        {
            Cheeps.Add(new CheepDTO(cheep));
        }


        Following = [];
        foreach (var a in author.Following)
        {
            Following.Add(a.DisplayName);
        }
    }
}