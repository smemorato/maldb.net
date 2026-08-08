using System.Security.Cryptography.X509Certificates;

namespace MyEfModels.Entities;

public class AnnStaff
{
    public int Id { get; set; }
    public int AnnAnimeId { get; set; }
    public int AnnPersonId { get; set; }
    public required string Position { get; set; }

    public AnnAnime Anime { get; set; } = null!;
    public AnnPerson Person { get; set; } = null!;

}
