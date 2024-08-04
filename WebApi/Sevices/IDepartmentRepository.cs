using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Sevices
{
    public interface IDepartmentRepository
    {
        List<Department> GetAll();
        Department Getone(int id);
        Department Getbyname(string name);
        int ADD(Department newdept);
        int Remove(int id);
        int update(Department newdept, int id);
        DepartmentWithEmployeeName getdetalis(int id);
    }
}