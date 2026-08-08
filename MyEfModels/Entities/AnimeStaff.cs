using System.Security.Cryptography.X509Certificates;

namespace MyEfModels.Entities;

public class AnimeStaff
{
    public int Id { get; set; }
    public int AnimeId { get; set; }
    public int PersonId { get; set; }
    public required string Position { get; set; }

    public Anime Anime { get; set; } = null!;
    public Person Person { get; set; } = null!;

}
