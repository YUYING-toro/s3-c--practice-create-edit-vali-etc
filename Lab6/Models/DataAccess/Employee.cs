using System;
using System.Collections.Generic;

#nullable disable

namespace Lab6.Models.DataAccess
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeRoles = new HashSet<EmployeeRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}
