using EmployeeManagement.Models;
using System.Globalization;

namespace EmployeeManagement.Services;

public class CsvService
{
	public List<Employee> ImportFromCsv(string filePath)
	{
		var employees = new List<Employee>();

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("File CSV tidak ditemukan", filePath);
		}

		var lines = File.ReadAllLines(filePath);

		// Skip baris pertama (header)
		for (int i = 1; i < lines.Length; i++)
		{
			var line = lines[i].Trim();
			if (string.IsNullOrWhiteSpace(line))
				continue;

			var parts = line.Split(';');
			if (parts.Length < 3)
				continue;

			try
			{
				var employee = new Employee
				{
					Name = parts[0].Trim(),
					Salary = decimal.Parse(parts[1].Trim(), CultureInfo.InvariantCulture),
					Status = parts[2].Trim().Equals("yes", StringComparison.OrdinalIgnoreCase)
				};

				employees.Add(employee);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Gagal memproses baris {i + 1}: {ex.Message}");
			}
		}

		return employees;
	}

	public void ExportToCsv(List<Employee> employees, string filePath)
	{
		using var writer = new StreamWriter(filePath);

		// Write header
		writer.WriteLine("id;name;salary;status");

		// Write data
		foreach (var employee in employees)
		{
			var status = employee.Status ? "yes" : "no";
			writer.WriteLine($"{employee.Id};{employee.Name};{employee.Salary.ToString(CultureInfo.InvariantCulture)};{status}");
		}
	}
}