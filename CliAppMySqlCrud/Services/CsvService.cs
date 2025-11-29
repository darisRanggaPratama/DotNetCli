using CliAppMySqlCrud.Models;
using CliAppMySqlCrud.Repositories;

namespace CliAppMySqlCrud.Services;

public class CsvService
{
    private readonly EmployeeRepository _repository;
    private readonly string _desktopPath;

    public CsvService(EmployeeRepository repository)
    {
        _repository = repository;
        _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }

    public (bool success, string message, int imported) ImportFromCsv(string filename)
    {
        try
        {
            string filePath = Path.Combine(_desktopPath, filename);

            if (!File.Exists(filePath))
            {
                return (false, $"File tidak ditemukan: {filePath}", 0);
            }

            var lines = File.ReadAllLines(filePath);

            if (lines.Length <= 1)
            {
                return (false, "File CSV kosong atau hanya berisi header", 0);
            }

            int imported = 0;
            int skipped = 0;
            var errors = new List<string>();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var parts = lines[i].Split(';');

                if (parts.Length < 4)
                {
                    skipped++;
                    errors.Add($"Baris {i + 1}: Format tidak valid");
                    continue;
                }

                try
                {
                    var employee = new Employee
                    {
                        Id = parts[0].Trim(),
                        Name = parts[1].Trim(),
                        Salary = decimal.Parse(parts[2].Trim()),
                        Status = parts[3].Trim() == "1" || parts[3].Trim().ToLower() == "yes"
                    };

                    if (string.IsNullOrWhiteSpace(employee.Id) || employee.Id.Length > 6)
                    {
                        skipped++;
                        errors.Add($"Baris {i + 1}: ID tidak valid (maksimal 6 digit)");
                        continue;
                    }

                    var existing = _repository.GetById(employee.Id);
                    if (existing != null)
                    {
                        skipped++;
                        errors.Add($"Baris {i + 1}: ID {employee.Id} sudah ada");
                        continue;
                    }

                    if (_repository.Create(employee))
                    {
                        imported++;
                    }
                    else
                    {
                        skipped++;
                        errors.Add($"Baris {i + 1}: Gagal menyimpan data");
                    }
                }
                catch (Exception ex)
                {
                    skipped++;
                    errors.Add($"Baris {i + 1}: {ex.Message}");
                }
            }

            string errorMessage = errors.Count > 0 ? $"\n\nError:\n{string.Join("\n", errors.Take(10))}" : "";
            if (errors.Count > 10)
            {
                errorMessage += $"\n... dan {errors.Count - 10} error lainnya";
            }

            return (true, $"Import selesai. Berhasil: {imported}, Dilewati: {skipped}{errorMessage}", imported);
        }
        catch (Exception ex)
        {
            return (false, $"Error saat import: {ex.Message}", 0);
        }
    }

    public (bool success, string message) ExportToCsv(string filename)
    {
        try
        {
            string filePath = Path.Combine(_desktopPath, filename);

            var employees = _repository.GetAll();

            if (employees.Count == 0)
            {
                return (false, "Tidak ada data untuk di-export");
            }

            using var writer = new StreamWriter(filePath);

            writer.WriteLine("id;name;salary;status");

            foreach (var emp in employees)
            {
                string status = emp.Status ? "1" : "0";
                writer.WriteLine($"{emp.Id};{emp.Name};{emp.Salary};{status}");
            }

            return (true, $"Data berhasil di-export ke: {filePath}");
        }
        catch (Exception ex)
        {
            return (false, $"Error saat export: {ex.Message}");
        }
    }
}