namespace Api.Mal.Dtos.AnimeDetails;

public class RelatedAnimeDto
{
    public RelatedAnimeNodeDto? Node { get; set; }
    public string? Relation_Type { get; set; }
    public string? Relation_Type_Formatted { get; set; }
}
