using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MySqlCLIapp.Models;

namespace MySqlCLIapp.Services
{
    public static class CsvService
    {
        public static IEnumerable<Employee> ReadEmployeesFromCsv(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);

            // Baca header terlebih dahulu agar GetField("nama_kolom") bisa digunakan
            if (csv.Read())
            {
                csv.ReadHeader();
            }

            var records = new List<Employee>();
            while (csv.Read())
            {
                var id = csv.GetField<string>("id");
                var name = csv.GetField<string>("name");
                var salaryStr = csv.GetField<string>("salary");
                var statusStr = csv.GetField<string>("status");

                if (string.IsNullOrWhiteSpace(id) || id.Length > 6 || !id.All(char.IsDigit))
                    continue; // skip invalid id
                if (!decimal.TryParse(salaryStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var salary))
                    continue;
                if (!int.TryParse(statusStr, out var status) || (status != 0 && status != 1))
                    continue;

                records.Add(new Employee
                {
                    Id = id.Trim(),
                    Name = name?.Trim() ?? string.Empty,
                    Salary = salary,
                    Status = status
                });
            }
            return records;
        }

        public static void WriteEmployeesToCsv(string path, IEnumerable<Employee> employees)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            };

            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, config);
            csv.WriteField("id");
            csv.WriteField("name");
            csv.WriteField("salary");
            csv.WriteField("status");
            csv.NextRecord();

            foreach (var e in employees)
            {
                csv.WriteField(e.Id);
                csv.WriteField(e.Name);
                csv.WriteField(e.Salary.ToString(CultureInfo.InvariantCulture));
                csv.WriteField(e.Status);
                csv.NextRecord();
            }
        }
    }
}