using CrudSpectreSearch.Services;
using Spectre.Console;

namespace CrudSpectreSearch.UI
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
				AnsiConsole.Write(new FigletText("Employee Manager").Centered().Color(Color.Blue));

				var choice = AnsiConsole.Prompt(
					new SelectionPrompt<string>()
						.Title("[green]Select an option:[/]")
						.AddChoices(new[]
						{
							"View All Employees",
							"Search Employees",
							"Add Employee",
							"Update Employee",
							"Delete Employee",
							"Exit"
						}));

				switch (choice)
				{
					case "View All Employees":
						ViewAllEmployees();
						break;
					case "Search Employees":
						SearchEmployees();
						break;
					case "Add Employee":
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
			table.Border(TableBorder.Rounded);
			table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
			table.AddColumn(new TableColumn("[yellow]Name[/]"));
			table.AddColumn(new TableColumn("[yellow]Salary[/]").RightAligned());
			table.AddColumn(new TableColumn("[yellow]State[/]").Centered());

			foreach (var emp in employees)
			{
				var stateColor = emp.Status ? "green" : "red";
				table.AddRow(
					emp.Id.ToString(),
					emp.Name,
					emp.Salary.ToString("C"),
					$"[{stateColor}]{emp.StatusDisplay}[/]"
				);
			}

			AnsiConsole.Write(table);
			AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
			Console.ReadKey();
		}

		private void AddEmployee()
		{
			Console.Clear();
			AnsiConsole.MarkupLine("[green]Add New Employee[/]\n");

			var name = AnsiConsole.Ask<string>("Enter employee [blue]name[/]:");
			var salary = AnsiConsole.Ask<decimal>("Enter employee [blue]salary[/]:");
			var status = AnsiConsole.Confirm("Is the employee [blue]active[/]?");

			_employeeService.CreateEmployee(name, salary, status);

			AnsiConsole.MarkupLine("\n[green]Employee added successfully![/]");
			AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
			Console.ReadKey();
		}

		private void SearchEmployees()
		{
			Console.Clear();
			AnsiConsole.MarkupLine("[green]Search Employees[/]\n");

			var searchType = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[blue]Search by:[/]")
					.AddChoices(new[]
					{
						"Name",
						"Salary Range",
						"Status (Active/Inactive)",
						"Back to Main Menu"
					}));

			if (searchType == "Back to Main Menu")
				return;

			IEnumerable<Models.Employee> results = new List<Models.Employee>();

			switch (searchType)
			{
				case "Name":
					var name = AnsiConsole.Ask<string>("Enter [blue]name[/] to search:");
					results = _employeeService.SearchEmployeesByName(name);
					break;

				case "Salary Range":
					var minSalary = AnsiConsole.Ask<decimal>("Enter [blue]minimum salary[/]:");
					var maxSalary = AnsiConsole.Ask<decimal>("Enter [blue]maximum salary[/]:");
					results = _employeeService.SearchEmployeesBySalaryRange(minSalary, maxSalary);
					break;

				case "Status (Active/Inactive)":
					var status = AnsiConsole.Confirm("Search for [blue]active[/] employees?");
					results = _employeeService.SearchEmployeesByStatus(status);
					break;
			}

			Console.Clear();
			AnsiConsole.MarkupLine($"[green]Search Results for: {searchType}[/]\n");

			var table = new Table();
			table.Border(TableBorder.Rounded);
			table.AddColumn(new TableColumn("[yellow]ID[/]").Centered());
			table.AddColumn(new TableColumn("[yellow]Name[/]"));
			table.AddColumn(new TableColumn("[yellow]Salary[/]").RightAligned());
			table.AddColumn(new TableColumn("[yellow]Status[/]").Centered());

			var resultsList = results.ToList();

			if (resultsList.Count == 0)
			{
				AnsiConsole.MarkupLine("[red]No employees found matching your criteria.[/]");
			}
			else
			{
				foreach (var emp in resultsList)
				{
					var stateColor = emp.Status ? "green" : "red";
					table.AddRow(
						emp.Id.ToString(),
						emp.Name,
						emp.Salary.ToString("C"),
						$"[{stateColor}]{emp.StatusDisplay}[/]"
					);
				}

				AnsiConsole.Write(table);
				AnsiConsole.MarkupLine($"\n[blue]Total: {resultsList.Count} employee(s) found[/]");
			}

			AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
			Console.ReadKey();
		}

		private void UpdateEmployee()
		{
			Console.Clear();
			var id = AnsiConsole.Ask<int>("Enter employee [blue]ID[/] to update:");

			var employee = _employeeService.GetEmployee(id);
			if (employee == null)
			{
				AnsiConsole.MarkupLine("[red]Employee not found![/]");
				AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
				Console.ReadKey();
				return;
			}

			AnsiConsole.MarkupLine($"\nCurrent data: [yellow]{employee.Name}[/] - [yellow]{employee.Salary:C}[/] - [yellow]{employee.StatusDisplay}[/]\n");

			var name = AnsiConsole.Ask("Enter new [blue]name[/]:", employee.Name);
			var salary = AnsiConsole.Ask("Enter new [blue]salary[/]:", employee.Salary);
			var status = AnsiConsole.Confirm("Is the employee [blue]active[/]?", employee.Status);

			_employeeService.UpdateEmployee(id, name, salary, status);

			AnsiConsole.MarkupLine("\n[green]Employee updated successfully![/]");
			AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
			Console.ReadKey();
		}

		private void DeleteEmployee()
		{
			Console.Clear();
			var id = AnsiConsole.Ask<int>("Enter employee [blue]ID[/] to delete:");

			var employee = _employeeService.GetEmployee(id);
			if (employee == null)
			{
				AnsiConsole.MarkupLine("[red]Employee not found![/]");
				AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
				Console.ReadKey();
				return;
			}

			AnsiConsole.MarkupLine($"\n[yellow]Employee: {employee.Name}[/]");
			var confirm = AnsiConsole.Confirm("[red]Are you sure you want to delete this employee?[/]");

			if (confirm)
			{
				_employeeService.DeleteEmployee(id);
				AnsiConsole.MarkupLine("\n[green]Employee deleted successfully![/]");
			}
			else
			{
				AnsiConsole.MarkupLine("\n[yellow]Deletion cancelled.[/]");
			}

			AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
			Console.ReadKey();
		}
	}
}
