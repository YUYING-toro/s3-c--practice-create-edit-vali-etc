using Lab6.Models.DataAccess;

namespace Lab6.Models
{
    public class RoleSelection //tie data > a class to collect selected checkBox
    {
        public Role role { get; set; }
        public bool selected { get; set; }

        //default
        public RoleSelection() 
        {
            role = null;
            selected = false;
        }

        //tie data
        public RoleSelection(Role role, bool selected = false) 
        {
            this.role = role;
            this.selected = selected;
        }
    }
}
