namespace CrudSpectreSearch.Models
{
	public class Employee
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Salary { get; set; }
		public bool Status { get; set; }
		public string StatusDisplay => Status ? "Yes" : "No";
	}
}
