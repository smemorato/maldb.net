
using Api.Tenrai.Dtos.Anime;
using Api.Tenrai.Dtos.CharacterDetails;

namespace Api.Tenrai.Dtos.PersonDetails;

public class TenraiPersonVoiceDto
{
    public required TenraiAnimeEntryDto Anime { get; set; }
    public required TenraiCharacterEntryDto Character { get; set; }
    public required string Role { get; set; }
    

}