using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Staff
{
    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlElement("task")]
    public required string Task { get; set; }

    [XmlElement("person")]
    public required Person Person { get; set; }
}

