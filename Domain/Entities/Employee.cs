namespace Domain.Entities;

public class Employee:Person
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Salary { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
    public int ManagerId { get; set; }
    public Employee Manager { get; set; }
    public DateTime HireDate { get; set; }

    public Employee(int id, string email, string password,string firstname, string lastname):base(firstname,lastname) 
    {
        Id = id;
        Email = email;
    }

    public Employee()
    {
        
    }
   
}