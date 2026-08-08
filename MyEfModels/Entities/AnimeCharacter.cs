namespace MyEfModels.Entities;

public class AnimeCharacter
{
    public int Id { get; set; }   // PRIMARY KEY

    public int AnimeId { get; set; }
    public Anime Anime { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; }

    public string Role { get; set; } = "";
    public int Favorites { get; set; }

    public ICollection<AnimeCharacterVoiceActor> VoiceActors { get; set; } = new List<AnimeCharacterVoiceActor>();
}

