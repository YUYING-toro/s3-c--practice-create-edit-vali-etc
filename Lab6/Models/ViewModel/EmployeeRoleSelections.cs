using Lab6.Models.DataAccess;
using System.Collections.Generic;


namespace Lab6.Models
{
    public class EmployeeRoleSelections
    {
        public Employee employee { get; set; }
        public List<RoleSelection> roleSelections { get; set; }  //select check box
        public EmployeeRoleSelections()
        {
            employee = new Employee();
            roleSelections = new List<RoleSelection>();
            //招喚 > 裝東西進去
            StudentRecordContext context = new StudentRecordContext();  //using using Lab6.Models.DataAccess; 有錯
            foreach ( Role role in context.Roles) 
            {
                RoleSelection roleSelection = new RoleSelection(role);
                roleSelections.Add(roleSelection);
            }

        }
    }
}
