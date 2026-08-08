using Api.Tenrai.Dtos;

namespace Api.Tenrai.Dtos.PersonDetails;

public class TenraiPersonDetailsDto
{
    public int Mal_Id { get; set; }
    public string? Url { get; set; }
    public string? Website_Url { get; set; }
    public TenraiImagesDto? Images { get; set; }
    public required string Name { get; set; }
    public string? Given_Name { get; set;}
    public string? Family_Name { get; set; }
    public List<string> Alternate_Names { get; set; } = new List<string>();
    public string? Birthday { get; set; }
    public int Favorites { get; set; }
    public string? About{ get; set; }
    public List<TenraiPersonAnimeDto> Anime { get; set; } = new List<TenraiPersonAnimeDto>();
    public List<TenraiPersonVoiceDto> Voices { get; set; } = new List<TenraiPersonVoiceDto>();
}