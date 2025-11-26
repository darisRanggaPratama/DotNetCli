namespace CrudCLIspectre.Services
{
	public class EmployeeService
	{
		private readonly IEmployeeRepository _repository;
		public EmployeeService(IEmployeeRepository repository)
		{
			_repository = repository;
		}
		public void CreateEmployee(string name, decimal salary, bool status)
		{
			var employee = new Models.Employee
			{
				Name = name,
				Salary = salary,
				Status = status
			};
			_repository.Add(employee);
		}
		public IEnumerable<Models.Employee> GetAllEmployees()
		{
			return _repository.GetAll();
		}
		public Models.Employee? GetEmployeeById(int id)
		{
			return _repository.GetById(id);
		}
		
		public bool UpdateEmployee(int id, string name, decimal salary, bool status)
		{
			var employee = _repository.GetById(id);
			if (employee == null)
			{
				return false;
			}
			employee.Name = name;
			employee.Salary = salary;
			employee.Status = status;
			_repository.Update(employee);
			return true;
		}

		public bool DeleteEmployee(int id)
		{
			var employee = _repository.GetById(id);
			if (employee == null)
			{
				return false;
			}
			_repository.Delete(id);
			return true;
		}
	}
}
