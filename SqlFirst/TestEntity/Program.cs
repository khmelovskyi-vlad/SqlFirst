using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            Initializer initializer = new Initializer();
            await initializer.Initialize();
            using (var db = new UniversityContext())
            {
                //var students = await db.Students.Where(studentt => studentt.Group.Course.Name == 1).Include(studentt => studentt.Group).ToListAsync();

                var courseeeeeeee = (await db.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 1 })).Entity;
                var courseee = db.Courses.Remove(await db.Courses.Where(coursee => coursee.Name == 1).FirstAsync());
                await db.SaveChangesAsync();
               
                var course = await db.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 1 });
                var specialty = await db.Specialties.AddAsync(new Specialty() { Id = Guid.NewGuid(), Name = "kib" });
                var subject = await db.Subjects.AddAsync(new Subject() { Id = Guid.NewGuid(), Name = "ec" });
                var subCo = await db.SubjectCourses.AddAsync(new SubjectCourse() { Course = course.Entity, Subject = subject.Entity,
                    CourseId = course.Entity.Id, SubjectId = subject.Entity.Id});
                var subSpec = await db.SubjectSpecialties.AddAsync(new SubjectSpecialty() { Specialty = specialty.Entity, Subject = subject.Entity,
                SpecialtyId = specialty.Entity.Id, SubjectId = subject.Entity.Id});
                var group = await db.Groups.AddAsync(new Group() { Id = Guid.NewGuid(), Name = "1-kib", Specialty = specialty.Entity, Course = course.Entity,
                SpecialtyId = specialty.Entity.Id, CourseId = course.Entity.Id});
                var student = db.Students.Add(new Student() { Id = Guid.NewGuid(), FirstName = "firstName", LastName = "lastName", AverageScore = null, Group = group.Entity,
                GroupId = group.Entity.Id});
                var score = db.Scores.AddAsync(new Score() { Id = Guid.NewGuid(), Subject = subject.Entity, Course = course.Entity, Student = student.Entity, Value = 5,
                CourseId = course.Entity.Id, StudentId = student.Entity.Id, SubjectId = subject.Entity.Id});
                await db.SaveChangesAsync();
            }

            return 1;

            //using (var db = new BloggingContext())
            //{
            //    db.Database.EnsureDeleted();
            //    //var blogs = db.Blogs
            //    //    .Where(b => b.Rating > 3)
            //    //    .OrderBy(b => b.Url)
            //    //    .ToList();
            //}
            //using (var db = new BloggingContext())
            //{
            //    var strategy = db.Database.CreateExecutionStrategy();

            //    strategy.Execute(() =>
            //    {
            //        using (var context = new BloggingContext())
            //        {
            //            using (var transaction = context.Database.BeginTransaction())
            //            {
            //                context.Blogs.Add(new Student { Url = "http://blogs.msdn.com/dotnet" });
            //                context.SaveChanges();

            //                context.Blogs.Add(new Student { Url = "http://blogs.msdn.com/visualstudio" });
            //                context.SaveChanges();

            //                transaction.Commit();
            //            }
            //        }
            //    });
            //}
            Console.ReadKey();
        }
    }
}
