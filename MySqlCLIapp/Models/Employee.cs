namespace MySqlCLIapp.Models
{
    public class Employee
    {
        public int RowId { get; set; }
        public string Id { get; set; } = string.Empty; // max 6 digits
        public string Name { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int Status { get; set; } // 1=yes, 0=no
    }
}