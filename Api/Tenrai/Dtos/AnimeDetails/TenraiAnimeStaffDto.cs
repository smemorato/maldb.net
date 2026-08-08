namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeStaffDto
{
    public required TenraiPersonEntryDto Person { get; set; }
    public List<string> Positions { get; set; } = new List<string>();
    
}