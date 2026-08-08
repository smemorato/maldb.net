using System.Runtime.CompilerServices;

namespace MyEfModels.Entities;

public class AnnAnime
{
    public int Id { get; set; }
    public required int AnnId { get; set; }
    public required string Title { get; set; }
    public List<string> AlternativeTitles { get; set; } = new List<string> {};
    public List<string> Genres { get; set; } = new List<string> {};
    public List<string> Themes { get; set; } = new List<string> {};
    public string? PlotSummary { get; set; }
    public int RunningTime { get; set; }
    public int? NumberEpisodes { get; set; }
    public string? Vintage { get; set; }
    public List<string> Openings { get; set; } = new List<string> {};
    public List<string> Endings { get; set; } = new List<string> {};
    public List<string> Websites { get; set; } = new List<string> {};
    public List<string> Episodes { get; set; } = new List<string> {};
    public List<string> Releases { get; set; } = new List<string> {};


    

    public ICollection<AnnStaff> Staff { get; set; } = new List<AnnStaff>();
    public ICollection<AnnStaff> Cast { get; set; } = new List<AnnStaff>();


}
