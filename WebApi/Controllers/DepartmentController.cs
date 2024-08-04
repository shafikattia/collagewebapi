using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Sevices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        [HttpGet] //api/Department
        public IActionResult getall()
        {
            var result = departmentRepository.GetAll();
            return Ok(result);
        }

         [HttpGet("{id:int}" , Name = "getoneDeptRoute")] //api/Department/1   Ambiguous by constrain ==> int
       //  [Route("{id:int}")] // Ambiguous by constrain ==> int
        public IActionResult getbyid(int id)
        {
            var result = departmentRepository.Getone(id);
            return Ok(result);
        }

        [HttpGet("{name:alpha}")] //api/Department/name  // Ambiguous by constrain ==> alpha
       
        public IActionResult Getbyname(string name)
        {
            var result = departmentRepository.Getbyname(name);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult insert(Department newdept)
        {
            if(ModelState.IsValid == true)
            { 
                departmentRepository.ADD(newdept);

                // how get current domain
               string url = Url.Link("getoneDeptRoute", new { id = newdept.Id });
                return Created(url, newdept);
              
            }
            //return BadRequest("department is not valid");
            return BadRequest(ModelState);

        }
        [HttpPut("{id}")]
        public IActionResult update([FromBody]Department newdept, [FromRoute]int id)
        {
            if(ModelState.IsValid == true)
            {
                departmentRepository.update(newdept, id);
                string url = Url.Link("getoneDeptRoute", new { id = id });
                return Created(url, newdept);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            departmentRepository.Remove(id);
            return Ok("removed");
        }

        [HttpGet("emp/{id:int}")]
        public IActionResult getdetalis(int id)
        {
            return Ok(departmentRepository.getdetalis(id));
        }
    }
}
