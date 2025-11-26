namespace MySqlCLI.Models;

public class Employee
{
    public int RowId { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public bool Status { get; set; }

    public string StatusDisplay => Status ? "Yes" : "No";
}