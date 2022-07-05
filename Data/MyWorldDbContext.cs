
using Microsoft.EntityFrameworkCore;
using ManageEmployeeAPI.Models;
using ManageEmployee.DTO;
using ManageEmployee.Models;

namespace Dot6.API.Crud.Data;

public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<User> Account { get; set; }  // tên database
}




