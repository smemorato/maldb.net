
using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Episode
{
    [XmlAttribute("num")]
    public int Number { get; set; }

    [XmlElement("title")]
    public required EpisodeTitle Title { get; set; }
}

public class EpisodeTitle
{
    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlAttribute("lang")]
    public string? Lang { get; set; }

    [XmlText]
    public string? Value { get; set; }
}
