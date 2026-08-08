using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class News
{
    [XmlAttribute("datetime")]
    public DateTime DateTime { get; set; }

    [XmlAttribute("href")]
    public string? Href { get; set; }

    [XmlText]
    public string? Title { get; set; }
}
