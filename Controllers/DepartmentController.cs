using Dot6.API.Crud.Data;
using ManageEmployeeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;

namespace ManageEmployee.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly MyWorldDbContext _configuration;

        private readonly IWebHostEnvironment _env;
        public DepartmentController(MyWorldDbContext configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            var departments = await _configuration.Department.ToListAsync();
            return Ok(departments);
        }
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Department department)
        {
            var depart = await _configuration.Department.FirstOrDefaultAsync(i => i.DepartmentID == department.DepartmentID);

            if (depart == null)
            {
                _configuration.Department.Add(department);
                await _configuration.SaveChangesAsync();
                return Created($"/get-department-by-id?id={department.DepartmentID}", department);
            }
            else
            {
                return BadRequest("Department was available");
            }
        }

        [Route("edit")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(DepartmentDTO department)
        {
            var depart = await _configuration.Department.FirstOrDefaultAsync(i => i.DepartmentID == department.DepartmentID);

            if (depart != null)
            {
                depart.DepartmentID = department.DepartmentID;
                depart.DepartmentName = department.DepartmentName;
                await _configuration.SaveChangesAsync();
                return Ok(depart);
            }

            return BadRequest("No department found");

        }

        [Route("{id}")]

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var departmentDelete = await _configuration.Department.FindAsync(id);
            if (departmentDelete == null)
            {
                return NotFound();
            }
            _configuration.Department.Remove(departmentDelete);
            await _configuration.SaveChangesAsync();
            return Ok("Deleted");
        }

    }

}
