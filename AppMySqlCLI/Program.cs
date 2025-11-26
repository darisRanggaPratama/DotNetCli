using Microsoft.Extensions.Configuration;
using MySqlCLI.Controllers;
using MySqlCLI.Data;

namespace MySqlCLI;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string tidak ditemukan dalam appsettings.json");
            return;
        }

        var dbContext = new DatabaseContext(connectionString);
        var controller = new EmployeeController(dbContext);

        controller.Run();
    }
}