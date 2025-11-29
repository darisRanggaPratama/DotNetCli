using System;
using Spectre.Console;
using MySqlCLIapp.Controllers;
using MySqlCLIapp.Repositories;
using MySqlCLIapp.Views;

namespace MySqlCLIapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var repo = new MySqlEmployeeRepository();
            var view = new EmployeeView();
            var controller = new EmployeeController(repo, view);

            while (true)
            {
                // Bersihkan layar sebelum menampilkan menu utama
                Console.Clear();
                
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Employee Manager[/] - pilih aksi:")
                        .PageSize(10)
                        .AddChoices(new[]
                        {
                            "Tampilkan semua",
                            "Cari berdasarkan ID",
                            "Cari berdasarkan Nama",
                            "Cari berdasarkan Salary",
                            "Cari berdasarkan Status",
                            "Tambah karyawan",
                            "Update karyawan (berdasarkan ID)",
                            "Hapus karyawan (berdasarkan ID)",
                            "Upload CSV dari Desktop",
                            "Download CSV ke Desktop",
                            "Keluar"
                        }));

                try
                {
                    switch (choice)
                    {
                        case "Tampilkan semua":
                            controller.ListAll();
                            break;
                        case "Cari berdasarkan ID":
                            controller.SearchById();
                            break;
                        case "Cari berdasarkan Nama":
                            controller.SearchByName();
                            break;
                        case "Cari berdasarkan Salary":
                            controller.SearchBySalary();
                            break;
                        case "Cari berdasarkan Status":
                            controller.SearchByStatus();
                            break;
                        case "Tambah karyawan":
                            controller.CreateEmployee();
                            break;
                        case "Update karyawan (berdasarkan ID)":
                            controller.UpdateEmployee();
                            break;
                        case "Hapus karyawan (berdasarkan ID)":
                            controller.DeleteEmployee();
                            break;
                        case "Upload CSV dari Desktop":
                            controller.UploadCsvFromDesktop();
                            break;
                        case "Download CSV ke Desktop":
                            controller.DownloadCsvToDesktop();
                            break;
                        case "Keluar":
                            return;
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Terjadi error:[/] {ex.Message}");
                }

                AnsiConsole.MarkupLine("\nTekan [green]Enter[/] untuk kembali ke menu...");
                Console.ReadLine();
            }
        }
    }
}