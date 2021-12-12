using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Lab6.Models.DataAccess
{
    public partial class StudentRecordContext : DbContext
    {
        public StudentRecordContext()
        {
        }

        public StudentRecordContext(DbContextOptions<StudentRecordContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicRecord> AcademicRecords { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer();

                //To access the DbContext outside controllers
                var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json");
                IConfiguration Configuration = builder.Build();
                string connectionString = Configuration.GetConnectionString("StudentRecord");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AcademicRecord>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseCode })
                    .HasName("PK__Academic__3D0525997BEA9AC0");

                entity.ToTable("AcademicRecord");

                entity.Property(e => e.StudentId).HasMaxLength(16);

                entity.Property(e => e.CourseCode).HasMaxLength(16);

                entity.HasOne(d => d.CourseCodeNavigation)
                    .WithMany(p => p.AcademicRecords)
                    .HasForeignKey(d => d.CourseCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AcademicRecord_Course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.AcademicRecords)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AcademicRecord_Student");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Course__A25C5AA6BDA575F9");

                entity.ToTable("Course");

                entity.Property(e => e.Code).HasMaxLength(16);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.RoleId });

                entity.ToTable("Employee_Role");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeRoles)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToEmployee");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToRole");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => new { e.CourseCourseId, e.StudentStudentNum })
                    .HasName("PK__Registra__92ECCCE9A5C002DA");

                entity.ToTable("Registration");

                entity.Property(e => e.CourseCourseId)
                    .HasMaxLength(16)
                    .HasColumnName("Course_CourseID");

                entity.Property(e => e.StudentStudentNum)
                    .HasMaxLength(16)
                    .HasColumnName("Student_StudentNum");

                entity.HasOne(d => d.CourseCourse)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.CourseCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToCourse");

                entity.HasOne(d => d.StudentStudentNumNavigation)
                    .WithMany(p => p.Registrations)
                    .HasForeignKey(d => d.StudentStudentNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToStudent");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Role1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.Id).HasMaxLength(16);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
