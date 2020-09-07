using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class UniversityContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectCourse> SubjectCourses { get; set; }
        public DbSet<SubjectSpecialty> SubjectSpecialties { get; set; }
        public DbSet<StudentScoresCount> StudentScoresCounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(new SqlConnection(GetSqlConnectionStringBuilder().ConnectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentScoresCount>(ssc => ssc.ToView("Viev_StudentScoresCount"));
            #region Keys

            modelBuilder.Entity<SubjectSpecialty>()
                .HasKey(ss => new { ss.SubjectId, ss.SpecialtyId });
            modelBuilder.Entity<SubjectCourse>()
                .HasKey(sc => new { sc.SubjectId, sc.CourseId });

            //modelBuilder.Entity<Student>()
            //    .HasOne(s => s.Group)
            //    .WithMany(g => g.Students)
            //    .HasForeignKey(s => s.GroupId);

            //modelBuilder.Entity<Group>()
            //    .HasOne(g => g.Course)
            //    .WithMany(c => c.Groups)
            //    .HasForeignKey(g => g.CourseId);

            //modelBuilder.Entity<Group>()
            //    .HasOne(g => g.Specialty)
            //    .WithMany(s => s.Groups)
            //    .HasForeignKey(g => g.SpecialtyId);

            //modelBuilder.Entity<Score>()
            //    .HasOne(s => s.Subject)
            //    .WithMany(s => s.Scores)
            //    .HasForeignKey(s => s.SubjectId);

            modelBuilder.Entity<Score>()
                .HasOne(s => s.Student)
                .WithMany(s => s.Scores)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Score>()
            //    .HasOne(s => s.Course)
            //    .WithMany(c => c.Scores)
            //    .HasForeignKey(s => s.CourseId);

            //modelBuilder.Entity<SubjectCourse>()
            //    .HasOne(sc => sc.Subject)
            //    .WithMany(s => s.SubjectCourses)
            //    .HasForeignKey(sc => sc.SubjectId);

            //modelBuilder.Entity<SubjectCourse>()
            //    .HasOne(sc => sc.Course)
            //    .WithMany(c => c.SubjectCourses)
            //    .HasForeignKey(sc => sc.CourseId);


            //modelBuilder.Entity<SubjectSpecialty>()
            //    .HasOne(ss => ss.Subject)
            //    .WithMany(s => s.SubjectSpecialties)
            //    .HasForeignKey(ss => ss.SubjectId);

            //modelBuilder.Entity<SubjectSpecialty>()
            //    .HasOne(ss => ss.Specialty)
            //    .WithMany(s => s.SubjectSpecialties)
            //    .HasForeignKey(ss => ss.SpecialtyId);
            #endregion

            #region Indexes
            modelBuilder.Entity<Score>()
                .HasIndex(s => s.Value);
            //.IncludeProperties(s => new
            //{
            //    s.StudentId,
            //    s.SubjectId
            //});
            #endregion
            //modelBuilder.Entity<Group>()
            //    .Property(g => g.Name)
            //    .HasComputedColumnSql("");
        }
        private SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = "WIN-DHV0BQSLTCR";
            sqlConnectionStringBuilder.UserID = "SQLFirst";
            sqlConnectionStringBuilder.Password = "Test1234";
            sqlConnectionStringBuilder.InitialCatalog = "University";
            return sqlConnectionStringBuilder;
        }
    }
}
