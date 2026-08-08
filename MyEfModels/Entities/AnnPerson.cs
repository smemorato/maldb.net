namespace MyEfModels.Entities;

public class AnnPerson
{
    public int Id { get; set; }
    public required int AnnId { get; set; }
    public string? Name { get; set; }


    public ICollection<AnnStaff> Staff { get; set; } = new List<AnnStaff>();
    public ICollection<AnnStaff> Cast { get; set; } = new List<AnnStaff>();

}
