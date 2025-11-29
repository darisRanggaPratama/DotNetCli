using MySqlSpectreCLI.Models;
using System.Text;

namespace MySqlSpectreCLI.Services
{

	public class CsvService
	{
		public List<Employee> ReadFromCsv(string filePath)
		{
			var employees = new List<Employee>();

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException($"File tidak ditemukan: {filePath}");
			}

			var lines = File.ReadAllLines(filePath);

			// Skip baris pertama (header)
			for (int i = 1; i < lines.Length; i++)
			{
				var line = lines[i].Trim();
				if (string.IsNullOrWhiteSpace(line))
					continue;

				var parts = line.Split(';');
				if (parts.Length < 4)
					continue;

				try
				{
					var employee = new Employee
					{
						Id = parts[0].Trim(),
						Name = parts[1].Trim(),
						Salary = decimal.Parse(parts[2].Trim()),
						Status = parts[3].Trim() == "1"
					};

					// Validasi ID maksimal 6 digit
					if (employee.Id.Length <= 6)
					{
						employees.Add(employee);
					}
				}
				catch (Exception)
				{
					// Skip baris yang error
					continue;
				}
			}

			return employees;
		}

		public void WriteToCsv(string filePath, List<Employee> employees)
		{
			var sb = new StringBuilder();

			// Header
			sb.AppendLine("id;name;salary;status");

			// Data
			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.Id};{employee.Name};{employee.Salary};{(employee.Status ? "1" : "0")}");
			}

			File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
		}
	}
}
