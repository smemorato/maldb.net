namespace Api.Mal.Dtos.AnimeDetails;
public class RelatedAnimeNodeDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public MainPictureDto? Main_Picture { get; set; }
}
