
namespace Api.Tenrai.Dtos.CharacterDetails;

public class TenraiCharacterAnimeDto
{
    public required TenraiAnimeEntryDto Anime { get; set; }
    public required string Role { get; set; }
    public int Favorites { get; set; }
    

}