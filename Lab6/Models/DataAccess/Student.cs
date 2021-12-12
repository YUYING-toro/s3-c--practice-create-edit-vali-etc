using System;
using System.Collections.Generic;

#nullable disable

namespace Lab6.Models.DataAccess
{
    public partial class Student
    {
        public Student()
        {
            AcademicRecords = new HashSet<AcademicRecord>();
            Registrations = new HashSet<Registration>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
