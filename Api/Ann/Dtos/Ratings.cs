using System.Xml.Serialization;


namespace Api.Ann.Dtos;

public class Ratings
{
    [XmlAttribute("nb_votes")]
    public int Votes { get; set; }

    [XmlAttribute("weighted_score")]
    public double WeightedScore { get; set; }

    [XmlAttribute("bayesian_score")]
    public double BayesianScore { get; set; }
}
