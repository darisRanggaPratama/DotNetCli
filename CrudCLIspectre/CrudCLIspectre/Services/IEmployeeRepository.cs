namespace CrudCLIspectre.Services
{
	public interface IEmployeeRepository
	{
		void Add(Models.Employee employee);
		IEnumerable<Models.Employee> GetAll();
		Models.Employee? GetById(int id);
		void Update(Models.Employee employee);
		void Delete(int id);
	}
}
