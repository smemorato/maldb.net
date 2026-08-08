using Api.Tenrai.Dtos;
using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.CharacterDetails;

public class TenraiCharacterDetailsDto
{
    public int Mal_Id { get; set; }
    public string? Url { get; set; }
    public TenraiImagesDto? Images { get; set; }
    public required string Name { get; set; }
    public string? Name_Kanji { get; set; }
    public List<string> NickNames { get; set; } = new List<string>();
    public int Favorites { get; set; }
    public string? About{ get; set; }
    public List<TenraiCharacterAnimeDto> Anime { get; set; } = new List<TenraiCharacterAnimeDto>();
}
    // Manga and voices are not implemented yet