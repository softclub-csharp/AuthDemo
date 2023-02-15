namespace Domain.Entities;

public class Person
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    public Person(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }

    public Person()
    {
        
    }
}