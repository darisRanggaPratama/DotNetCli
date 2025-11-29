using CliAppMySqlCrud.Controllers;
using CliAppMySqlCrud.Data;
using CliAppMySqlCrud.Repositories;
using CliAppMySqlCrud.Services;
using CliAppMySqlCrud.Views;
using Spectre.Console;

namespace EmployeeManagement;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            AnsiConsole.Write(
                new FigletText("Employee System")
                    .Centered()
                    .Color(Color.Cyan));

            AnsiConsole.MarkupLine("[grey]Initializing database...[/]");

            var dbConnection = new DatabaseConnection();
            dbConnection.InitializeDatabase();

            var repository = new EmployeeRepository(dbConnection);
            var employeeService = new EmployeeService(repository);
            var csvService = new CsvService(repository);
            var view = new ConsoleView();

            var controller = new EmployeeController(employeeService, csvService, view);

            AnsiConsole.MarkupLine("[green]✓ Database ready![/]");
            Thread.Sleep(1000);

            controller.Run();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Fatal Error: {ex.Message}[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.StackTrace}[/]");
            Console.ReadKey();
        }
    }
}