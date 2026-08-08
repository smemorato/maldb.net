namespace Api.Mal.Dtos;


public class AlternativeTitlesDto
{
    public List<string> Synonyms { get; set; } = new();
    public string? En { get; set; }
    public string? Ja { get; set; }
}