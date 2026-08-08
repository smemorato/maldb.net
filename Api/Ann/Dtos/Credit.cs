using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Credit
{
    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlElement("task")]
    public string? Task { get; set; }

    [XmlElement("company")]
    public required Company Company { get; set; }
}
