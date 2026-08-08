namespace Api.Tenrai.Dtos.Anime;

public class TenraiCharacterEntryDto
{
    public int  Mal_Id { get; set; }
    public required string Url { get; set; }
    public TenraiImagesDto? Images { get; set; }
    public required string Name { get; set; }
}