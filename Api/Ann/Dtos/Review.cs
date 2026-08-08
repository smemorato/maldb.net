using System.Xml.Serialization;


namespace Api.Ann.Dtos;
public class Review
{
    [XmlAttribute("href")]
    public string? Href { get; set; }

    [XmlText]
    public string? Title { get; set; }
}
