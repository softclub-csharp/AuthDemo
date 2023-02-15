namespace Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    public int? ManagerId { get; set; }
    public Employee Manager { get; set; }
    public int  LocationId { get; set; }
    
}