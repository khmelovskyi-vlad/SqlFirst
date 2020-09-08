using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Program
    {
        private static object Foo(Score score, string sv)
        {
            if (sv == "Value")
            {
                return score.Value;
            }
            else
            {
                return score.Id;
            }
        }
        static async Task<int> Main(string[] args)
        {
            using (var db = new UniversityContext())
            {
                var par1 = new SqlParameter("@studentsCount", 10);
                var s = db.Students.FromSqlRaw("ShowSomeStudents @studentsCount", par1).ToList();
                foreach (var s1 in s)
                {
                    Console.WriteLine(s1.FirstName);
                }
            }
            var userInteractor = new ConsoleUserInteractor();
            CommandMaster commandMaster = new CommandMaster(userInteractor, new Initializer(userInteractor));
            await commandMaster.Run();
            using (var db = new UniversityContext())
            {
                var subjects = await db.Subjects
                    .Where(subject => subject.SubjectCourses != null && subject.SubjectSpecialties != null)
                    .Include(subject => subject.SubjectCourses)
                    .ThenInclude(subjectCourse => subjectCourse.Course)
                    .Include(subject => subject.SubjectSpecialties)
                    .ThenInclude(subjectSpecialty => subjectSpecialty.Specialty)
                    .ToListAsync();
                var courses = await db.Courses.Include(course => course.SubjectCourses).ThenInclude(sc => sc.Subject).ToListAsync();
                var specialties = await db.Specialties.Include(spec => spec.SubjectSpecialties).ThenInclude(ss => ss.Subject).ToListAsync();
                var allSt = 0;
                foreach (var course in courses)
                {
                    foreach (var specialty in specialties)
                    {
                        var l = course.SubjectCourses.Join(specialty.SubjectSpecialties,
                            subCo => subCo.Subject,
                            subSpec => subSpec.Subject,
                            (subCo, subspec) => new { course = subCo.Course.Name, spec = subspec.Specialty.Name, subject = subCo.Subject.Name });
                        allSt = allSt + l.Count();
                    }
                }
                var res = courses.Join(specialties,
                    course => course.SubjectCourses.Select(subCo => subCo.SubjectId),
                    spec => spec.SubjectSpecialties.Select(ss => ss.SubjectId),
                    (course, spec) => new
                    {
                        course,
                        spec
                    }).ToList();
                Console.WriteLine(allSt);
                var allSt2 = 0;
                var result = subjects.SelectMany(subject => subject.SubjectCourses.Join(subject.SubjectSpecialties,
                        subCo => subCo.Subject,
                        subSpec => subSpec.Subject,
                        (subCo, subspec) => new { course = subCo.Course.Name, spec = subspec.Specialty.Name, subject = subCo.Subject.Name }));
                Console.WriteLine(result.Count());
                foreach (var subject in subjects)
                {
                    var l = subject.SubjectCourses.Join(subject.SubjectSpecialties,
                        subCo => subCo.Subject,
                        subSpec => subSpec.Subject,
                        (subCo, subspec) => new { course = subCo.Course.Name, spec = subspec.Specialty.Name, subject = subCo.Subject.Name });
                    allSt2 = allSt2 + l.Count();
                }
                Console.WriteLine(allSt2);
                //foreach (var item in l)
                //{
                //    Console.WriteLine($"{item.course}-{item.spec}-{item.subject}");
                //}

                //var oih = subjects.Select(sub => sub.SubjectCourses.).toList();
                //var sub = subjects.GroupBy(
                //    subject => subject.SubjectCourses,
                //    subject => subject.SubjectSpecialties,
                //    (subCo, subSpec) => new
                //    {
                //        course = subCo.Select(scc => scc.Course),
                //        specialty = subSpec(ss => ss.)
                //    }
                //    );
                //var scores = db.Scores.ToList();
                //foreach (var score in scores)
                //{
                //    Console.WriteLine(score.Value);
                //}
                //Console.WriteLine("next");
                //var sc = scores[0];
                //var sv = nameof(sc.Value);
                //var needScores = scores.OrderBy(score =>Foo(score,sv)).Take(200);
                //foreach (var score in needScores)
                //{
                //    Console.WriteLine(score.Value);
                //}
                Console.ReadLine();
            }
                using (var db = new UniversityContext())
            {
                //var result = db.StudentScoresCounts.ToList();
                //foreach (var res in result)
                //{
                //    Console.WriteLine(res.Id);
                //    Console.WriteLine(res.FirstName);
                //    Console.WriteLine(res.LastName);
                //    Console.WriteLine(res.ScoreCount);
                //}
                //var scores = db.Database.ExecuteSqlRawAsync("SELECT * FROM [dbo].[Scores]").ToList();
                var par1 = new SqlParameter("@studentsCount", 10);
                var s = db.Students.FromSqlRaw("ShowSomeStudents @studentsCount", par1).ToList();
                var par = new SqlParameter("@maxFoursCount", 10);
                var cleverStudent = db.Students.FromSqlRaw("SELECT * FROM GetCleverStudents(@maxFoursCount)", par).ToList();
                var command = "CREATE FUNCTION [dbo].[GetCleverStudents] " +
                            "( " +
                            "@maxFoursCount int " +
                            ") " +
                            "RETURNS TABLE AS RETURN " +
                            "( " +
                            "SELECT DISTINCT st.Id AS Id, stud.FirstName AS FirstName, stud.LastName AS LastName, stud.[AverageScore], g.[Id] AS GroupId " +
                            "FROM( " +
                            "SELECT stt.Id, MIN(scoree.[Value]) AS MinimalScore " +
                            "FROM [dbo].[Scores] scoree " +
                            "JOIN [dbo].[Students] stt ON stt.Id = scoree.StudentId " +
                            "JOIN [dbo].[Groups] gg ON gg.Id = stt.GroupId " +
                            "JOIN [dbo].[Subjects] subb ON subb.Id = scoree.SubjectId " +
                            "JOIN [dbo].[SubjectCourses] subCoo ON subCoo.SubjectId = subb.Id AND gg.CourseId = subCoo.CourseId " +
                            "JOIN [dbo].[SubjectSpecialties] subSpecc ON subSpecc.SubjectId = subb.Id AND gg.SpecialtyId = subSpecc.SpecialtyId " +
                            "WHERE scoree.CourseId = gg.CourseId " +
                            "GROUP BY stt.ID) st " +
                            "JOIN (SELECT st.Id, score.[Value] AS[Score], COUNT(score.[Id]) AS[Count] " +
                            "FROM [dbo].[Scores] score " +
                            "JOIN [dbo].[Students] st ON st.Id = score.StudentId " +
                            "JOIN [dbo].[Groups] g ON g.Id = st.GroupId " +
                            "JOIN [dbo].[Subjects] sub ON sub.Id = score.SubjectId " +
                            "JOIN [dbo].[SubjectCourses] subCo ON subCo.SubjectId = sub.Id AND g.CourseId = subCo.CourseId " +
                            "JOIN [dbo].[SubjectSpecialties] subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId " +
                            "WHERE score.CourseId = g.CourseId " +
                            "GROUP BY st.Id, score.[Value]) ts ON ts.Id = st.Id " +
                            "JOIN [dbo].[Students] stud ON stud.Id = ts.Id " +
                            "JOIN [dbo].[Groups] g ON g.Id = stud.GroupId " +
                            "WHERE CAST(1 AS BIT) LIKE CAST(CASE WHEN ts.[Count] <= @maxFoursCount AND st.MinimalScore = 4 THEN 1 ELSE 0 END AS BIT) " +
                            "OR st.MinimalScore = 5 " +
                            ")";
                var lol = db.Database.ExecuteSqlRaw(command);
                //var sss = await db.Database.("[dbo].[RandIntBetween] " +
                //    "FROM [dbo].[Students]");
                //var ss = await db.Database.ExecuteSqlRawAsync("SELECT * " +
                //    "FROM [dbo].[Students]", par);
                //var s = db.Students.FromSqlRaw("ShowSomeStudents '1'", par).ToList();
            }
            //using (var db = new UniversityContext())
            //{
            //    db.Subjects.RemoveRange(db.Subjects);
            //    await db.SaveChangesAsync();
            //    db.Courses.RemoveRange(db.Courses);
            //    await db.SaveChangesAsync();
            //    db.Specialties.RemoveRange(db.Specialties);
            //    await db.SaveChangesAsync();
            //}
            Initializer initializer = new Initializer(new ConsoleUserInteractor());
            await initializer.FirstInitializeData();
            //await initializer.ChangeScores();
            //await initializer.AddScore();
            await initializer.FirstInitializeData();
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
