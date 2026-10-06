namespace Api.Tenrai.Dtos.Anime;

public class TenraiUserDto
{
    public required string Url { get; set; }
    public required string Username { get; set; }

    public TenraiImagesDto? Images { get; set; }

}