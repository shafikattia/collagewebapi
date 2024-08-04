using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Sevices
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ItiDbContext context;

        public EmployeeRepository(ItiDbContext _context)
        {
            context = _context;
        }
        public List<EmployeeWithDepartmentNameDTO> Getallwithdept() 
        {
            List<Employee> emp = context.Employees.Include(e => e.Department).ToList(); 
            List< EmployeeWithDepartmentNameDTO > EMPDTO = new List<EmployeeWithDepartmentNameDTO>();
            if (emp != null )
            {

                foreach (var item in emp)
                {
                    var dto = new EmployeeWithDepartmentNameDTO
                    {
                        Id = item.Id,
                        EmployeeName = item.Name,
                        EmployeeAddress = item.Address,
                        DepartmentNmae = item.Department.Name
                    };
                    EMPDTO.Add(dto);
                }


                return EMPDTO;
            }

            return null; 


        }
        public EmployeeWithDepartmentNameDTO Getonewithdeptame(int id)
        {
            Employee emp = context.Employees.Include(e=>e.Department).FirstOrDefault(e=>e.Id == id);
            if (emp != null)
            {
                EmployeeWithDepartmentNameDTO EMPDTO = new EmployeeWithDepartmentNameDTO();

                EMPDTO.Id=emp.Id;
                EMPDTO.EmployeeName = emp.Name;
                EMPDTO.EmployeeAddress=emp.Address;
                EMPDTO.DepartmentNmae = emp.Department.Name;

                return EMPDTO;
            }
            return null;
        }
        
        public int ADD(Employee newdept)
        {
            context.Employees.Add(newdept);
            var row = context.SaveChanges();
            return row;

        }
        public int Remove(int id)
        {
            Employee emp = context.Employees.Find(id);
            context.Remove(emp);
            var row = context.SaveChanges();
            return row;

        }
        public int update(Employee neweEmp, int id)
        {
            Employee oldemp = context.Employees.Find(id);  

            oldemp.Name = neweEmp.Name;
            oldemp.Address = neweEmp.Address;
            oldemp.Phone = neweEmp.Phone;
            oldemp.DepartmentId = neweEmp.DepartmentId;
            var result = context.SaveChanges();
            return result;

        }
    }
}
