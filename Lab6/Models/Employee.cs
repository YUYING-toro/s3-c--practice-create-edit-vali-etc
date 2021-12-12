using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
//外頭 Use partial class to tie the generated Employee class to EmployeeMetadata class

namespace Lab6.Models.DataAccess //沒有指定 DataAccess 找沒 List<的Roleles>
{
    [ModelMetadataType(typeof(metadataEmp))]
    public partial class Employee
    {
        [NotMapped]
        public List<Role> Roles
        {
            get
            {
                List<Role> roles = new List<Role>();
                using (StudentRecordContext context = new StudentRecordContext())
                {
                    roles = (from r in context.Roles where context.EmployeeRoles.Any(er => er.RoleId == r.Id && er.EmployeeId == r.Id) select r).ToList<Role>();
                }
                return roles;
            }
        }
    }
}
