using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Initializer
    {
        public Initializer()
        {

        }
        public async Task AddRandomStudent()
        {
            //using (var context = new UniversityContext())
            //{
            //    await context.Students
            //}
        }
        private async Task<Course[]> AddCourses(UniversityContext context)
        {
            Course[] courses = new Course[]
            {
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 1 })).Entity,
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 2 })).Entity,
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 3 })).Entity,
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 4 })).Entity,
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 5 })).Entity,
                    (await context.Courses.AddAsync(new Course() { Id = Guid.NewGuid(), Name = 6 })).Entity
            };
            return courses;
        }
        private async Task<Specialty[]> AddSpecialties(UniversityContext context, Random random)
        {
            Specialty[] specialties = new Specialty[80];
            for (int i = 0; i < specialties.Length; i++)
            {
                specialties[i] = (await context.Specialties.AddAsync(new Specialty()
                {
                    Id = Guid.NewGuid(),
                    Name = CreateRandomString(0, 50, "abcdefghijklmnopqrstuvwxyz", random)
                })).Entity;
            }
            return specialties;
        }
        private async Task<Subject[]> AddSubjects(UniversityContext context, Random random)
        {
            Subject[] subjects = new Subject[1000];
            for (int i = 0; i < subjects.Length; i++)
            {
                subjects[i] = (await context.Subjects.AddAsync(new Subject()
                {
                    Id = Guid.NewGuid(),
                    Name = CreateRandomString(0, 50, "abcdefghijklmnopqrstuvwxyz", random)
                })).Entity;
            }
            return subjects;
        }
        private async Task<SubjectCourse[]> AddSubjectCourses(UniversityContext context, Subject[] subjects, Course[] courses, Random random)
        {
            List<SubjectCourse> subjectCourses = new List<SubjectCourse>();
            for (int i = 0; i < subjects.Length; i++)
            {
                for (int j = 0; j < courses.Length; j++)
                {
                    if (CreateRandomBool(random, 50))
                    {
                        subjectCourses.Add((await context.SubjectCourses.AddAsync(new SubjectCourse()
                        {
                            Subject = subjects[i],
                            SubjectId = subjects[i].Id,
                            Course = courses[j],
                            CourseId = courses[j].Id
                        })).Entity);
                    }
                }
            }
            return subjectCourses.ToArray();
        }
        private async Task<SubjectSpecialty[]> AddSubjectSpecialties(UniversityContext context, Subject[] subjects, Specialty[] specialties, Random random)
        {
            List<SubjectSpecialty> subjectSpecialties = new List<SubjectSpecialty>();
            for (int i = 0; i < subjects.Length; i++)
            {
                for (int j = 0; j < specialties.Length; j++)
                {
                    if (CreateRandomBool(random, 10))
                    {
                        subjectSpecialties.Add((await context.SubjectSpecialties.AddAsync(new SubjectSpecialty()
                        {
                            Subject = subjects[i],
                            SubjectId = subjects[i].Id,
                            Specialty = specialties[j],
                            SpecialtyId = specialties[j].Id,
                        })).Entity);
                    }
                }
            }
            return subjectSpecialties.ToArray();
        }
        private async Task<Group[]> AddGroups(UniversityContext context, Course[] courses, Specialty[] specialties)
        {
            List<Group> groups = new List<Group>();
            for (int i = 0; i < courses.Length; i++)
            {
                for (int j = 0; j < specialties.Length; j++)
                {
                    groups.Add((await context.Groups.AddAsync(new Group()
                    {
                        Id = Guid.NewGuid(),
                        Name = $"{courses[i].Name} - {specialties[j].Name}",
                        Course = courses[i],
                        CourseId = courses[i].Id,
                        Specialty = specialties[j],
                        SpecialtyId = specialties[j].Id
                    })).Entity);
                }
            }
            return groups.ToArray();
        }

        private async Task<Student[]> AddStudents(UniversityContext context, Group[] groups, Random random)
        {
            Student[] students = new Student[100000];
            for (int i = 0; i < students.Length; i++)
            {
                var indexGroup = random.Next(0, groups.Length);
                students[i] = ((await context.Students.AddAsync(new Student()
                {
                    Id = Guid.NewGuid(),
                    FirstName = CreateRandomString(0, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    LastName = CreateRandomString(0, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    Group = groups[indexGroup],
                    GroupId = groups[indexGroup].Id
                })).Entity);
            }
            return students.ToArray();
        }
        private async Task<Score[]> AddScores(UniversityContext context, Student[] students, Subject[] subjects, Random random)
        {
            List<Score> scores = new List<Score>();
            for (int i = 0; i < students.Length; i++)
            {
                var needSubjects = subjects.Where(subjectss => subjectss.SubjectCourses != null 
                    && subjectss.SubjectSpecialties != null
                    && subjectss.SubjectCourses.Select(subCo => subCo.Course).Contains(students[i].Group.Course)
                    && subjectss.SubjectSpecialties.Select(subSpec => subSpec.Specialty).Contains(students[i].Group.Specialty));

                foreach (var subject in needSubjects)
                {
                    if (CreateRandomBool(random, 5))
                    {
                        scores.Add((await context.Scores.AddAsync(new Score()
                        {
                            Id = Guid.NewGuid(),
                            Student = students[i],
                            StudentId = students[i].Id,
                            Subject = subject,
                            SubjectId = subject.Id,
                            Course = students[i].Group.Course,
                            CourseId = students[i].Group.Course.Id,
                            Value = random.Next(0, 6)
                        })).Entity);
                    }
                }
            }
            return scores.ToArray();
        }
        public async Task Initialize()
        {
            Random random = new Random();
            using (var context = new UniversityContext())
            {
                var courses = await AddCourses(context);
                var specialties = await AddSpecialties(context, random);
                var subjects = await AddSubjects(context, random);
                var subjectSpecialties = await AddSubjectSpecialties(context, subjects, specialties, random);
                var subjectCourses = await AddSubjectCourses(context, subjects, courses, random);
                var groups = await AddGroups(context, courses, specialties);
                var students = await AddStudents(context, groups, random);
                var scores = await AddScores(context, students, subjects, random);
                await context.SaveChangesAsync();
            }
        }


        private string CreateRandomString(int minLength, int maxLenght, string chars, Random random)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < random.Next(minLength, maxLenght); i++)
            {
                stringBuilder.Append(chars[random.Next(0, chars.Length)]);
            }
            return stringBuilder.ToString();
        }
        private bool CreateRandomBool(Random random, int percent)
        {
            if (percent > random.Next(1, 101)) 
            {
                return true;
            }
            return false;
        }
    }
}
