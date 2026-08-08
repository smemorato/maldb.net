using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Info
{
    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("lang")]
    public string? Lang { get; set; }

    [XmlAttribute("src")]
    public string? Src { get; set; }

    [XmlAttribute("href")]
    public string? Href { get; set; }

    [XmlAttribute("width")]
    public int Width { get; set; }

    [XmlAttribute("height")]
    public int Height { get; set; }

    [XmlText]
    public string? Value { get; set; }
}
