namespace Domain.Entities;

public class Location
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }    
    public string State { get; set; }
    public string ZipCode { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; }
}