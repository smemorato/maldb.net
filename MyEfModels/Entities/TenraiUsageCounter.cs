    namespace MyEfModels.Entities;
public class TenraiUsageCounter
{
    public int Id { get; set; }
    public int Count { get; set; }
    public DateTime Date { get; set; }  // stored as UTC
}
