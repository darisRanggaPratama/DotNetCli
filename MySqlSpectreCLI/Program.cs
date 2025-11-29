using MySqlSpectreCLI.Controllers;
namespace MySqlSpectreCLI
{

	class Program
	{
		static void Main(string[] args)
		{
			var controller = new EmployeeController();
			controller.Run();
		}
	}
}

// Ubah kode agar edit dan hapus karyawan dalam bentuk table
