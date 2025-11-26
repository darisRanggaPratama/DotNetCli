using CrudSpectreSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudSpectreSearch.Services
{
	public interface IEmployeeRepository
	{
		void Add(Employee employee);
		IEnumerable<Employee> GetAll();
		Employee? GetById(int id);
		void Update(Employee employee);
		void Delete(int id);
		IEnumerable<Employee> SearchByName(string name);
		IEnumerable<Employee> SearchBySalaryRange(decimal minSalary, decimal maxSalary);
		IEnumerable<Employee> SearchByStatus(bool state);
	}
}
