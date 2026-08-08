using System.Xml.Serialization;



namespace Api.Ann.Dtos;
public class Anime
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("precision")]
    public string? Precision { get; set; }

    [XmlAttribute("generated-on")]
    public DateTime GeneratedOn { get; set; }

    [XmlElement("related-prev")]
    public List<Related> RelatedPrev { get; set; } = new();

    [XmlElement("info")]
    public List<Info> Info { get; set; } = new();

    [XmlElement("ratings")]
    public Ratings Ratings { get; set; } = new();

    [XmlElement("episode")]
    public List<Episode> Episodes { get; set; } = new();

    [XmlElement("review")]
    public List<Review> Reviews { get; set; } = new ();

    [XmlElement("release")]
    public List<Release> Releases { get; set; } = new();

    [XmlElement("news")]
    public List<News> News { get; set; } = new();

    [XmlElement("staff")]
    public List<Staff> Staff { get; set; } = new();

    [XmlElement("cast")]
    public List<Cast> Cast { get; set; } = new();

    [XmlElement("credit")]
    public List<Credit> Credits { get; set; } =new();
}
