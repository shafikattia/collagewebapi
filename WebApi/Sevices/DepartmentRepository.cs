using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Sevices
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ItiDbContext context;

        public DepartmentRepository(ItiDbContext _context)
        {
            context = _context;
        }
        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }
        public Department Getone(int id)
        {
            return context.Departments.Find(id);
        }
        public Department Getbyname(string name)
        {
            return context.Departments.FirstOrDefault(d=>d.Name == name);
        }
        public int ADD(Department newdept)
        {
           context.Departments.Add(newdept);
            var row = context.SaveChanges();
            return row ;

        }
        public int Remove(int id)
        {
            context.Remove(Getone(id));
            var row = context.SaveChanges();
            return row;

        }
        public int update(Department newdept , int id) 
        {
           var olddept = Getone(id);
            olddept.Name = newdept.Name;
            olddept.Manger = newdept.Manger;
            var result = context.SaveChanges();
            return result;
        
        }

        public DepartmentWithEmployeeName getdetalis(int id)
        {
            Department dep = context.Departments.Include(d => d.Employees)
                .FirstOrDefault(d => d.Id == id);

            if(dep != null)
            {
                DepartmentWithEmployeeName dptdto = new DepartmentWithEmployeeName();
                dptdto.Id = dep.Id;
                dptdto.DepartmentName = dep.Name;
                foreach(var item in  dep.Employees) 
                {
                    dptdto.EmployeeName.Add(item.Name);
                }

                return dptdto;
            }
            return null;

        }
    }
}
