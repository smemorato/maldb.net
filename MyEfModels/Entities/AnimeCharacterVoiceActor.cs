namespace MyEfModels.Entities;


public class AnimeCharacterVoiceActor
{
    public int Id { get; set; }   // You MUST add this PK

    public int AnimeCharacterId { get; set; }
    public AnimeCharacter AnimeCharacter { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }

    public string Language { get; set; } = "";
}


