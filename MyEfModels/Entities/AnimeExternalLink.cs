using MyEfModels.Migrations;

namespace MyEfModels.Entities;

public class AnimeExternalLink
{
    public int Id { get; set; }   // PRIMARY KEY

    public int AnimeId { get; set; }
    public Anime Anime { get; set; } = null!;
    public required string Name { get; set; }
    public required string Url { get; set; }
    public int? LinkId { get; set; }
}

