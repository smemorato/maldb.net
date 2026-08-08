
using System.Xml.Serialization;

namespace Api.Ann.Dtos;

public class Cast
{
    [XmlAttribute("gid")]
    public long Gid { get; set; }

    [XmlAttribute("lang")]
    public required string Lang { get; set; }

    [XmlElement("role")]
    public  required string Role { get; set; }

    [XmlElement("person")]
    public required Person Person { get; set; }
}
