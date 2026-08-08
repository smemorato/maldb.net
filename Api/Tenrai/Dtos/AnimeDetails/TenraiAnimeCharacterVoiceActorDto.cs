namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeCharacterVoiceActorDto
{
    public TenraiPersonEntryDto? Person { get; set; }
    public required string Language { get; set; }

}