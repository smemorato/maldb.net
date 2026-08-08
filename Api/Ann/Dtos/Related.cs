using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Related
{
    [XmlAttribute("rel")]
    public required string Relation { get; set; }

    [XmlAttribute("id")]
    public int Id { get; set; }
}
