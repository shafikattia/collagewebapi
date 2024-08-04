namespace WebApi.DTO
{
    public class DepartmentWithEmployeeName
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public List<string> EmployeeName { get; set; } = new List<string>();
    }
}
