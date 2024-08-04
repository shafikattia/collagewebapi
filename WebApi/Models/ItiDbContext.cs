using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class ItiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ItiDbContext() 
        { 
        }
        public ItiDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
