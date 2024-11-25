using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;

public class AuthorDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }

    public required List<AuthorDTO> Following { get; set; }
}