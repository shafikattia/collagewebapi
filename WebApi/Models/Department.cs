namespace WebApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Manger { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}
