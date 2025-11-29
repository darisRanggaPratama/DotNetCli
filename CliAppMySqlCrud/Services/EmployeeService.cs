using CliAppMySqlCrud.Models;
using CliAppMySqlCrud.Repositories;

namespace CliAppMySqlCrud.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService(EmployeeRepository repository)
    {
        _repository = repository;
    }

    public List<Employee> GetAllEmployees()
    {
        return _repository.GetAll();
    }

    public Employee? GetEmployeeById(string id)
    {
        return _repository.GetById(id);
    }

    public List<Employee> SearchEmployeesByName(string name)
    {
        return _repository.SearchByName(name);
    }

    public List<Employee> FilterEmployeesBySalary(decimal minSalary, decimal maxSalary)
    {
        return _repository.FilterBySalary(minSalary, maxSalary);
    }

    public List<Employee> FilterEmployeesByStatus(bool status)
    {
        return _repository.FilterByStatus(status);
    }

    public (bool success, string message) CreateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Id) || employee.Id.Length > 6)
        {
            return (false, "ID karyawan harus diisi dan maksimal 6 digit");
        }

        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            return (false, "Nama karyawan harus diisi");
        }

        if (employee.Salary < 0)
        {
            return (false, "Salary tidak boleh negatif");
        }

        var existingEmployee = _repository.GetById(employee.Id);
        if (existingEmployee != null)
        {
            return (false, "ID karyawan sudah ada");
        }

        bool result = _repository.Create(employee);
        return result ? (true, "Data karyawan berhasil ditambahkan") : (false, "Gagal menambahkan data karyawan");
    }

    public (bool success, string message) UpdateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Id))
        {
            return (false, "ID karyawan harus diisi");
        }

        var existingEmployee = _repository.GetById(employee.Id);
        if (existingEmployee == null)
        {
            return (false, "Data karyawan tidak ditemukan");
        }

        bool result = _repository.Update(employee);
        return result ? (true, "Data karyawan berhasil diupdate") : (false, "Gagal mengupdate data karyawan");
    }

    public (bool success, string message) DeleteEmployee(string id)
    {
        var existingEmployee = _repository.GetById(id);
        if (existingEmployee == null)
        {
            return (false, "Data karyawan tidak ditemukan");
        }

        bool result = _repository.Delete(id);
        return result ? (true, "Data karyawan berhasil dihapus") : (false, "Gagal menghapus data karyawan");
    }

    public (bool success, string message) DeleteAllEmployees()
    {
        bool result = _repository.DeleteAll();
        return result ? (true, "Semua data karyawan berhasil dihapus") : (false, "Gagal menghapus data karyawan");
    }
}