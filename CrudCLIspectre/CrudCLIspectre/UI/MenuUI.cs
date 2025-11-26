using CrudCLIspectre.Services;
using Spectre.Console;

namespace CrudCLIspectre.UI
{
	public class MenuUI
	{
		private readonly EmployeeService _employeeService;

		public MenuUI(EmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		public void Run()
		{
			while (true)
			{
				Console.Clear();
				AnsiConsole.Write(new FigletText("Employee Manager").Centered().Color(Color.Green));

				var choice = AnsiConsole.Prompt(
					new SelectionPrompt<string>()
						.Title("[green]Select an option:[/]")
						.AddChoices(new[]
						{
						"View All Employee",
						"Add New Employee",
						"Update Employee",
						"Delete Employee",
						"Exit"
						}));

				switch (choice)
				{
					case "View All Employee":
						ViewAllEmployees();
						break;
					case "Add New Employee":
						AddEmployee();
						break;
					case "Update Employee":
						UpdateEmployee();
						break;
					case "Delete Employee":
						DeleteEmployee();
						break;
					case "Exit":
						return;

				}
			}
		}

		private void ViewAllEmployees()
		{
			Console.Clear();
			var employees = _employeeService.GetAllEmployees();

			var table = new Table();
			table.Border = TableBorder.Rounded;
			table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
			table.AddColumn(new TableColumn("[yellow]Name[/]").Centered());
			table.AddColumn(new TableColumn("[yellow]Salary[/]").Centered());
			table.AddColumn(new TableColumn("[yellow]Status[/]").Centered());

			foreach (var emp in employees)
			{
				var statusColor = emp.Status ? "green" : "red";
				table.AddRow(
					emp.Id.ToString(),
					emp.Name,
					emp.Salary.ToString("C"),
				$"[{statusColor}]{emp.statusDisplay}[/]");
			}

			AnsiConsole.Write(table);
			AnsiConsole.MarkupLine("\nPress any key to continue...");
			Console.ReadKey();

		}

		private void AddEmployee()
		{
			Console.Clear();
			AnsiConsole.MarkupLine("[green]Add New Employee[/]\n");

			var name = AnsiConsole.Ask<string>("Enter employee [yellow]name[/]:");
			var salary = AnsiConsole.Ask<decimal>("Enter employee [yellow]salary[/]:");
			var status = AnsiConsole.Confirm("Is the employee [yellow]active[/]?");

			_employeeService.CreateEmployee(name, salary, status);

			AnsiConsole.MarkupLine("\n[green]Employee added successfully![/]");
			AnsiConsole.MarkupLine("\nPress any key to continue...");
			Console.ReadKey();
		}

		private void UpdateEmployee()
		{
			Console.Clear();
			var id = AnsiConsole.Ask<int>("Enter the [yellow]ID[/] of the employee to update:");

			var employee = _employeeService.GetEmployeeById(id);
			if (employee == null)
			{
				AnsiConsole.MarkupLine("[red]Employee not found![/]");
				AnsiConsole.MarkupLine("\nPress any key to continue...");
				Console.ReadKey();
				return;
			}

			AnsiConsole.MarkupLine($"\nCurrent data: [yellow]{employee.Name}[/] - [yellow]{employee.Salary:C}[/] - [yellow]{employee.statusDisplay}[/]\n");

			var name = AnsiConsole.Ask("Enter new [blue]name[/]:", employee.Name);
			var salary = AnsiConsole.Ask("Enter new [blue]salary[/]:", employee.Salary);
			var status = AnsiConsole.Confirm("Is the employee [blue]active[/]?", employee.Status);

			_employeeService.UpdateEmployee(id, name, salary, status);
			AnsiConsole.MarkupLine("\n[green]Employee updated successfully![/]");
			AnsiConsole.MarkupLine("\nPress any key to continue...");
			Console.ReadKey();
		}

		private void DeleteEmployee()
		{
			Console.Clear();
			var id = AnsiConsole.Ask<int>("Enter the [yellow]ID[/] of the employee to delete:");
			var employee = _employeeService.GetEmployeeById(id);
			if (employee == null)
			{
				AnsiConsole.MarkupLine("[red]Employee not found![/]");
				AnsiConsole.MarkupLine("\nPress any key to continue...");
				Console.ReadKey();
				return;
			}

			AnsiConsole.MarkupLine($"\n[yellow]Employee: {employee.Name}[/]");
			var confirm = AnsiConsole.Confirm($"Are you sure you want to delete employee [yellow]{employee.Name}[/]?");
			if (confirm)
			{
				_employeeService.DeleteEmployee(id);
				AnsiConsole.MarkupLine("\n[green]Employee deleted successfully![/]");
			}
			else
			{
				AnsiConsole.MarkupLine("\n[yellow]Deletion cancelled.[/]");
			}
			AnsiConsole.MarkupLine("\nPress any key to continue...");
			Console.ReadKey();
		}
	}
}
