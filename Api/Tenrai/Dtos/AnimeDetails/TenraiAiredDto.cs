namespace Api.Tenrai.Dtos.Anime;

public class TenraiAiredDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public required TenraiAiredPropDto Prop { get; set;}
    public string? AiredString { get; set; }
}