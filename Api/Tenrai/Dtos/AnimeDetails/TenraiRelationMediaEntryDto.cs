namespace Api.Tenrai.Dtos.Anime;
public class TenraiRelationMediaEntryDto
{
    public int Mal_id { get; set; }
    public required string Type { get; set; }
    public required string Name { get; set; }
    public required string Media_Type { get; set; }
    public TenraiImagesDto? Images { get; set; }

}