namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeCompanyDto
{
    public int Mal_Id { get; set; }
    public string?  Type { get; set; }
    public required string Name { get; set; }
    public string? Url { get; set; }

}