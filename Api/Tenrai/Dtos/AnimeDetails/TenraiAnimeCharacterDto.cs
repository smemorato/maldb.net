namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeCharacterDto
{
    public required TenraiCharacterEntryDto Character { get; set; }
    public required string Role { get; set; }
    public int Favorites { get; set; }
    public List<TenraiAnimeCharacterVoiceActorDto> Voice_Actors { get; set; } = new List<TenraiAnimeCharacterVoiceActorDto>();
    

}