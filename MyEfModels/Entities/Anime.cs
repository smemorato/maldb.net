namespace MyEfModels.Entities;

public class Anime
{
    public int Id { get; set; }
    public required int MalId { get; set; }

    public required string TitleRomanji { get; set; }
    public string? TitleEnglish {get; set;}
    public string? TitleOriginal { get; set; }
    public List<string> TitleSynonyms { get; set; } = new List<string> {};
    public string? MainPictureMedium {get; set; }
    public string? MainPictureLarge {get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Synopsis { get; set; }
    public Decimal? MeanScore {get; set;}
    public int? Rank { get; set; }
    public int Popularity { get; set; }
    public int ListUserCount { get; set; }
    public int ScoringUserCount { get; set; }
    public string? Nsfw { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? MediaType { get; set; }
    public string? Status { get; set; }
    public int? NumberEpisodes { get; set; }
    public int? StartSeasonYear { get; set; }
    public string? StartSeasonSeason { get; set; }
    public string? BroadcastWeekday {get; set; }
    public string? BroadcastTime { get; set; }
    public string? Source { get; set; }
    public int? EpisodeDuration { get; set; }
    public string? rating { get; set; }
    public int Favorites { get; set; }
    public ICollection<string> Pictures { get; set; } = new List<string>{};
    public string? BackgroundInformation { get; set; }
     public int WatchingCount { get; set; }
    public int CompletedCount { get; set; }
    public int OnHoldCount { get; set; }
    public int DroppedCount { get; set; }
    public int PlanToWatchCount { get; set; }
    public int forumTopicsCounter { get; set; }
    public DateOnly LastTenraiUpdate { get; set; }


    public ICollection<AnimeGenre> AnimeGenres { get; set; } =  new List<AnimeGenre> {};
    public ICollection<AnimeCompany> AnimeCompanies { get; set; }  = new List<AnimeCompany> {};
    public ICollection<AnimeRelation> RelationsFrom { get; set; } = new List<AnimeRelation> {};
    public ICollection<AnimeRelation> RelationsTo { get; set; } = new List<AnimeRelation> {};
    public ICollection<AnimeRecommendation> RecommendationsFrom { get; set; } 
        = new List<AnimeRecommendation>();
    public ICollection<AnimeRecommendation> RecommendationsTo { get; set; } 
        = new List<AnimeRecommendation>();

    public ICollection<AnimeTheme> AnimeThemes { get; set; } 
        = new List<AnimeTheme>();

    public ICollection<AnimeReview> AnimeReviews { get; set; }  = new List<AnimeReview> {};
    public ICollection<AnimeEpisode> AnimeEpisodes { get; set; }  = new List<AnimeEpisode> {};


    public ICollection<UserList> UserLists { get; set; } = new List<UserList>();
    public ICollection<AnimeStaff> Staff { get; set; } = new List<AnimeStaff>();
    public ICollection<AnimeCharacter> AnimeCharacters { get; set; } = new List<AnimeCharacter>();
    public ICollection<AnimeExternalLink> ExternalLinks { get; set; } = new List<AnimeExternalLink>();


}
