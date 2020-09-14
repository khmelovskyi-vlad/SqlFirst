using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class SqlMaster
    {
        public async Task<string> GetRandomString(SqlParameter[] parameters)
        {
            using (var university = new UniversityContext())
            {
                await university.Database.ExecuteSqlRawAsync("[dbo].[PickRandomString] @minLength, @maxLength, @chars, @randomString OUTPUT", parameters);
                return parameters.Where(parameter => parameter.ParameterName == "@randomString").First().Value.ToString();
            }
        }

        public async Task AddStudents(List<Student> students)
        {
            using (var university = new UniversityContext())
            {
                await university.Students.AddRangeAsync(students);
                await university.SaveChangesAsync();
            }
        }
        public async Task AddScores(List<Score> scores)
        {
            using (var university = new UniversityContext())
            {
                await university.Scores.AddRangeAsync(scores);
                await university.SaveChangesAsync();
            }
        }
        public async Task ChangeScores(List<Score> scores)
        {
            using (var university = new UniversityContext())
            {
                university.Scores.UpdateRange(scores);
                await university.SaveChangesAsync();
            }
        }
        //public async Task<List<StudentScoresCount>> GetStudentScoresCount()
        //{
        //    using (var university = new UniversityContext())
        //    {
        //        return await university.StudentScoresCounts
        //            .ToListAsync();
        //    }
        //}
        //public async Task<List<Student>> GetCleverStudents(SqlParameter sqlParameter)
        //{
        //    using (var university = new UniversityContext())
        //    {
        //        return await university.Students.FromSqlRaw("SELECT * " +
        //            "FROM GetCleverStudents(@maxFoursCount)", sqlParameter).ToListAsync();
        //    }
        //}
        public void AddStudent(Student student)
        {
            using (var universityContext = new UniversityContext())
            {
                var result = universityContext.Attach(student).State = EntityState.Added;
                universityContext.SaveChanges();
            }
        }
        public async Task<List<Student>> GetStudents()
        {
            using (var university = new UniversityContext())
            {
                return await university.Students
                    .Include(student => student.Group)
                    .ToListAsync();
            }
        }
        public async Task<List<Score>> GetScores()
        {
            using (var university = new UniversityContext())
            {
                return await university.Scores
                    .Include(score => score.Course)
                    .Include(score => score.Subject)
                    .Include(score => score.Student)
                    .ThenInclude(student => student.Group)
                    .ToListAsync();
            }
        }
        public async Task<List<Group>> GetGroups()
        {
            using (var university = new UniversityContext())
            {
                return await university.Groups
                    .Include(group => group.Course)
                    .Include(group => group.Specialty)
                    .ToListAsync();
            }
        }
        public async Task<List<Course>> GetCourses()
        {
            using (var university = new UniversityContext())
            {
                return await university.Courses.ToListAsync();
            }
        }
        public async Task<List<Subject>> GetSubjects()
        {
            using (var university = new UniversityContext())
            {
                return await university.Subjects
                    .Where(subject => subject.SubjectCourses != null && subject.SubjectSpecialties != null)
                    .Include(subject => subject.SubjectCourses)
                    .ThenInclude(subjectCourse => subjectCourse.Course)
                    .Include(subject => subject.SubjectSpecialties)
                    .ThenInclude(subjectSpecialty => subjectSpecialty.Specialty)
                    .ToListAsync();
            }
        }
        public async Task<List<Specialty>> GetSpecialties()
        {
            using (var university = new UniversityContext())
            {
                return await university.Specialties.ToListAsync();
            }
        }
        private async Task<List<Subject>> GetAllSubjects()
        {
            using (var university = new UniversityContext())
            {
                return await university.Subjects
                    .Where(subject => subject.SubjectCourses != null && subject.SubjectSpecialties != null)
                    .Include(subject => subject.SubjectCourses)
                    //.ThenInclude(subjectCourse => subjectCourse.Course)
                    .Include(subject => subject.SubjectSpecialties)
                    //.ThenInclude(subjectSpecialty => subjectSpecialty.Specialty)
                    .ToListAsync();
            }
        }
        public async Task<int> UpdateData(string sql)
        {
            using (var university = new UniversityContext())
            {
                return await university.Database.ExecuteSqlRawAsync(sql);
            }
        }
        public async Task Foo()
        {
            using (var university = new UniversityContext())
            {
                var specialty = university.Specialties.AsQueryable();
                var result = await OrderByy(specialty, "Name", true).ToListAsync();
            }
        }
        public IQueryable<TEntity> OrderByy<TEntity>(IQueryable<TEntity> source, string orderByProperty,
                         bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
