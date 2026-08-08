namespace Api.Tenrai.Dtos.Anime;

public class TenraiGenreDto
{
    public int Mal_id { get; set; }
    public string?  Type { get; set; }
    public required string Name { get; set; }
    public string? Url { get; set; }

}