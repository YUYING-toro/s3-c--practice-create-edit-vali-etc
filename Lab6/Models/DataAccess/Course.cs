using System;
using System.Collections.Generic;

#nullable disable

namespace Lab6.Models.DataAccess
{
    public partial class Course
    {
        public Course()
        {
            AcademicRecords = new HashSet<AcademicRecord>();
            Registrations = new HashSet<Registration>();
        }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? HoursPerWeek { get; set; }
        public decimal? FeeBase { get; set; }

        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
