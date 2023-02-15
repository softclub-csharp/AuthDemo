namespace Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public List<Employee> Employees { get; set; }
}