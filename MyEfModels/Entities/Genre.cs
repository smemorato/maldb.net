    namespace MyEfModels.Entities;

public class Genre
{
    public int Id { get; set; }
    public int MalId {get; set; }
    public required string Name { get; set; }
    public string? Type { get; set; }
    public string? Url { get; set; }
    public ICollection<AnimeGenre> AnimeGenre { get; set; } = new List<AnimeGenre> {};

}
