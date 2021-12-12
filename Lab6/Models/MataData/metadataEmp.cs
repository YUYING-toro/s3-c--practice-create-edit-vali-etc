using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab6.Models.DataAccess // same name model.cs
{
    public class metadataEmp
    {
        //name Employee Name must be in the form of first name last name
        [Display(Name = "Employee Name")]
        [Required]
        [RegularExpression("([A-Za-z]+)\\s([A-Za-z]+)", ErrorMessage = "Name must be in the form of first name last name")]
        public string Name { get; set; }

        //id Network ID must be unique and more than 3 characters.
        [Display(Name = "Employee ID")]
        [Required]
        [StringLength(51,MinimumLength =3,ErrorMessage = "ID must be unique and more than 3 characters.")]
        public string UserName { get; set; }  //userName


        //pp length must be more than 5 characters.
        [Display(Name = "Employee Password")]
        [Required]
        [StringLength(51, MinimumLength = 5, ErrorMessage = "Password must be more than 5 characters.")]
        public string Password { get; set; }
        //checkbox At least one role
        //[Display(Name = "Job Title")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "At least one role")]
        //public List<> 
    }
}
