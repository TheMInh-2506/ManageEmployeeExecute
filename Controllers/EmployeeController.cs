using Dot6.API.Crud.Data;
using ManageEmployee.DTO;
using ManageEmployeeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace ManageEmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly MyWorldDbContext _configuration;

        private readonly IWebHostEnvironment _env;
        public EmployeeController(MyWorldDbContext configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            var employee = await _configuration.Employee.ToListAsync();
            return Ok(employee);
        }
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Employee employee)
        {
            // var employ = await _configuration.Employee.FirstOrDefaultAsync(i => i.EmployeeID == employee.EmployeeID);

            // if (employ == null)
            // {
            _configuration.Employee.Add(employee);
            await _configuration.SaveChangesAsync();
            return Ok("Created Successfull");
            // }
            // else
            // {
            //     return BadRequest("Employee was available");
            // }
        }
        [Route("edit")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(EmployeeDTO employee)
        {
            var empl = await _configuration.Employee.FirstOrDefaultAsync(i => i.EmployeeID == employee.EmployeeID);

            if (empl != null)
            {
                empl.EmployeeID = employee.EmployeeID;
                empl.EmployeeName = employee.EmployeeName;
                empl.EmployeeImg = employee.EmployeeImg;
                empl.DateJoining = employee.DateJoining;
                empl.Department = employee.Department;
                await _configuration.SaveChangesAsync();
                return Ok("Edit Successfull");
            }

            return BadRequest("Employee not available");
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var employeeToDelete = await _configuration.Employee.FindAsync(id);
            if (employeeToDelete == null)
            {
                return NotFound();
            }
            _configuration.Employee.Remove(employeeToDelete);
            await _configuration.SaveChangesAsync();
            return Ok("Deleted");
        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }

}
