namespace CrudCLIspectre.Models
{
	public class Employee
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool Status { get; set; }
		public decimal Salary { get; set; }

		public string statusDisplay => Status ? "Active" : "Inactive";
	}
}
