using System.Xml.Serialization;



namespace Api.Ann.Dtos;

[XmlRoot("ann")]
public class Ann
{
    [XmlElement("anime")]
    public required Anime Anime { get; set; }
}
