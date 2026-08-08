namespace Api.Tenrai.Dtos.Anime;
public class TenraiAnimeDto
{
    public int Mal_Id { get; set; }
    public required string Url { get; set; }
    public TenraiImagesDto? Images { get; set; } 
    public TenraiVideoDto? Trailer{ get; set; }
    public bool Approved { get; set; }
    public required string Title { get; set; }
    public string? Title_English { get; set; }
    public string? Title_Japanese { get; set; }
    public List<string> Title_Synonyms { get; set; } = new List<string>();
    public string? Type { get; set; }
    public string? Source { get; set; }
    public int? Episodes { get; set; }
    public required string Status { get; set; }
    public bool Airing { get; set; }
    public TenraiAiredDto? Aired { get; set; }
    public string? Duration { get; set;}
    public string? ContentRating { get; set; }
    public decimal? Score { get; set; }
    public int Scored_By { get; set; }
    public int? Rank { get; set; }
    public int Popularity { get; set; }
    public int Members { get; set; }
    public int Favorites { get; set; }
    public string? Synopsis { get; set; }
    public string? Background { get; set; }
    public string? Season { get; set; }
    public int? Year { get; set; }
    public TenraiBroadcastDto? BroadcastDto { get; set; }
    public List<TenraiAnimeCompanyDto> Producers { get; set; } = new List<TenraiAnimeCompanyDto>();
    public List<TenraiAnimeCompanyDto> Licensors { get; set; } = new List<TenraiAnimeCompanyDto>();
    public List<TenraiAnimeCompanyDto> Studios { get; set; } = new List<TenraiAnimeCompanyDto>();
    public List<TenraiGenreDto> Genres { get; set; } = new List<TenraiGenreDto>();
    public List<TenraiGenreDto> Explicit_Genres { get; set; } = new List<TenraiGenreDto>();
    public List<TenraiGenreDto> Themes { get; set; } = new List<TenraiGenreDto>();
    public List<TenraiGenreDto> Demographics { get; set; } = new List<TenraiGenreDto>();
    public List<TenraiRelationsDto> Relations { get; set; } = new List<TenraiRelationsDto>();
    public TenraiThemeDto? Theme { get; set; }
    public List<TenraiExternalLinkDto> External { get; set; } = new List<TenraiExternalLinkDto>();
    public List<TenraiExternalLinkDto> Streaming { get; set; } = new List<TenraiExternalLinkDto>();
    public string? Moreinfo { get; set; }



    


}