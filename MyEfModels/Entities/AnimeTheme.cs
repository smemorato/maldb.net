namespace MyEfModels.Entities;

public class AnimeTheme
{
    public int Id { get; set; }
    public int AnimeId {get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public bool Updated { get; set; } = false; 

    public Anime? Anime { get; set; }
}