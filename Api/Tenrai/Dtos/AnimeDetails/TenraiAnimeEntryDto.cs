namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeEntryDto
{
    public int Mal_Id { get; set; }
    public required string Url { get; set; }
    public TenraiImagesDto? Images { get; set; }
}