using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Release
{
    [XmlAttribute("date")]
    public string? Date { get; set; }

    [XmlAttribute("href")]
    public string? Href { get; set; }

    [XmlText]
    public string? Title { get; set; }
}
