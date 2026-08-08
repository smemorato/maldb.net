    namespace MyEfModels.Entities;

public class AnimeGenre
{
    public int Id { get; set; }
    public int AnimeId {get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public Anime? Anime { get; set; }

}
