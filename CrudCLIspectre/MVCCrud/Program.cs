using EmployeeManagement.Controllers;

namespace EmployeeManagement;

class Program
{
	static void Main(string[] args)
	{
		using var controller = new EmployeeController();
		controller.Run();
	}
}