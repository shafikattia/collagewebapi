using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Employee
    {
         public int Id {  get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
