namespace CrudCli;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
    public bool Status { get; set; }
    
    public Employee(int id, string name, decimal salary, bool status)
    {
        Id = id;
        Name = name;
        Salary = salary;
        Status = status;
    }

    public override string ToString()
    {
        return $"ID: {Id} Name: {Name} Salary: {Salary} Status: {(Status ? "Active" : "Inactive")}";
    }
}