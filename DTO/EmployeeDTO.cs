using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageEmployee.DTO
{
    public class EmployeeDTO
    {
                public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int Department { get; set; }
        public string EmployeeImg { get; set; }
        public DateTime DateJoining { get; set; }
    }
}