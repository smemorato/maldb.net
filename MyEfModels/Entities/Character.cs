namespace MyEfModels.Entities;

public class Character
{
    public int Id { get; set; }
    public required int MalId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
    public string? NameKanji { get; set; }
    public List<string> Nicknames { get; set; } = new List<string>();
    public int Favorites { get; set; }
    public string? About { get; set; }

    public ICollection<AnimeCharacter> AnimeCharacters { get; set; } = new List<AnimeCharacter>();

}
