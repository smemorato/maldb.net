
namespace Api.Mal.Dtos.AnimeRanking;

public class RankingAnimeNodeDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public MainPictureDto? Main_Picture { get; set; }
    public AlternativeTitlesDto? Alternative_Titles { get; set; }
    public string? Start_Date { get; set; }
    public string? End_Date { get; set; }
    public string? Synopsis { get; set; }
    public decimal? Mean { get; set; }
    public int Rank { get; set; }
    public int Popularity { get; set; }
    public int Num_List_Users { get; set; }
    public int Num_Scoring_Users { get; set; }
    public string? Nsfw { get; set; }
    public DateTimeOffset Created_At { get; set; }
    public DateTimeOffset Updated_At { get; set; }
    public string? Media_Type { get; set; }
    public string? Status { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
    public int? Num_Episodes { get; set; }
    public StartSeasonDto? Start_Season { get; set; }
    public BroadcastDto? Broadcast { get; set; }
    public string? Source { get; set; }
    public int? Average_Episode_Duration { get; set; }
    public string? Rating { get; set; }
    public List<StudioDto> Studios { get; set; } = new();

}
