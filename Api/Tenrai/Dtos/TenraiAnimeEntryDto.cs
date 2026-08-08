using Api.Tenrai.Dtos;

namespace Api.Tenrai.Dtos;

public class TenraiAnimeEntryDto
{
    public int  Mal_Id { get; set; }
    public required string Url { get; set; }
    public TenraiImagesDto? Images { get; set; }
    public required string Title { get; set; }
}