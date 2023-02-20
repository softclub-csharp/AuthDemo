namespace Domain.Entities;

public class Country
{
    public int  Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }
    public Region Region { get; set; }
   
}