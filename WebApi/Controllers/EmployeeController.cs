using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Sevices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository _employeeRepository) 
        {
            employeeRepository = _employeeRepository;
        }
        [HttpGet]
        public IActionResult Getallwithdept()
        {
            return Ok(employeeRepository.Getallwithdept());
        }

        [HttpGet("{id:int}")]
        public IActionResult Getonewithdept(int id ) 
        {
            return Ok(employeeRepository.Getonewithdeptame(id));
        }
    }
}
