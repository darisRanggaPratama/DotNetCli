using Spectre.Console;
using MySqlCLIapp.Models;

namespace MySqlCLIapp.Views
{
    public class EmployeeView
    {
        public void RenderEmployees(IEnumerable<Employee> employees, string title)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.Title($"[bold]{title}[/]");
            table.AddColumn("Row ID");
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Salary");
            table.AddColumn("Status");

            foreach (var e in employees)
            {
                table.AddRow(
                    e.RowId.ToString(),
                    e.Id,
                    e.Name,
                    e.Salary.ToString("N2"),
                    e.Status == 1 ? "Yes" : "No"
                );
            }

            AnsiConsole.Write(table);
        }
    }
}