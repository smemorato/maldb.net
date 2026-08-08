using System.Xml.Serialization;


namespace Api.Ann.Dtos;


public class Company
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlText]
    public required string Name { get; set; }
}
