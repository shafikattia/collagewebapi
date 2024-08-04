using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Sevices
{
    public interface IEmployeeRepository
    {
        int ADD(Employee newdept);
        int Remove(int id);
        int update(Employee neweEmp, int id);
        EmployeeWithDepartmentNameDTO Getonewithdeptame(int id);
        List<EmployeeWithDepartmentNameDTO> Getallwithdept();
    }
}