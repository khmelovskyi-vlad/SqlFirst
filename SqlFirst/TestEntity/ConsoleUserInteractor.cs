using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace TestEntity
{
    class ConsoleUserInteractor : IUserInteractor
    {
        public List<Student> ReadStudents(List<Group> groups)
        {
            List<Student> students = new List<Student>();
            while (true)
            {
                var student = new Student();
                student.Id = Guid.NewGuid();
                student.FirstName = ReadData("Write a student first name");
                student.LastName = ReadData("Write a student last name");
                var group = SelectGroup(groups);
                student.GroupId = group.Id;
                students.Add(student);
                if (!CheckNeedAddMore("If you want to add more students, click 'Enter'"))
                {
                    return students;
                }
            }
        }
        private bool CheckNeedAddMore(string offer)
        {
            Console.WriteLine(offer);
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return true;
            }
            return false;
        }
        private Group SelectGroup(List<Group> groups)
        {
            while (true)
            {
                Console.WriteLine("If you want to watch groups, click 'Enter'");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    WriteGroups(groups);
                }
                var groupName = ReadData("Write a group name");
                foreach (var group in groups)
                {
                    if (group.Name == groupName)
                    {
                        return group;
                    }
                }
                Console.WriteLine("Don't have this group name, write else");
            }
        }
        private void WriteGroups(List<Group> groups)
        {
            Console.WriteLine("Group name | group course | group specialty");
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Name, -20} | {group.Course.Name} | {group.Specialty.Name}");
            }
        }
        private string ReadData(string offer)
        {
            while (true)
            {
                Console.WriteLine(offer);
                var line = Console.ReadLine();
                if (line.Length > 0)
                {
                    return line;
                }
                else
                {
                    Console.WriteLine("Bad input, try again");
                }
            }
        }

        public List<Score> ReadScores(List<Student> students, List<Subject> subjects)
        {
            List<Score> scores = new List<Score>();
            while (true)
            {
                var score = new Score();
                score.Id = Guid.NewGuid();
                var student = SelectStudent(students);
                score.StudentId = student.Id;
                score.CourseId = student.Group.CourseId;
                score.SubjectId = SelectSubject(GetStudentSubjects(student, subjects)).Id;
                score.Value = ReadScoreValue();
                scores.Add(score);
                if (!CheckNeedAddMore("If you want to add more students, click 'Enter'"))
                {
                    return scores;
                }
            }
        }
        private List<Subject> GetStudentSubjects(Student student, List<Subject> subjects)
        {
            return subjects
                .Where(subject => subject.SubjectCourses != null
                && subject.SubjectSpecialties != null
                && subject.SubjectCourses.Select(subjectCourse => subjectCourse.CourseId).Contains(student.Group.CourseId)
                && subject.SubjectSpecialties.Select(subjectSpecialty => subjectSpecialty.SpecialtyId).Contains(student.Group.SpecialtyId))
                .ToList();
        }
        private Subject SelectSubject(List<Subject> subjects)
        {
            while (true)
            {
                Console.WriteLine("If you want to watch all subjects, click 'Enter'");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    WriteAllSubjects(subjects);
                }
                var subjectName = ReadData("Write a subject name");
                foreach (var subject in subjects)
                {
                    if (subject.Name == subjectName)
                    {
                        return subject;
                    }
                }
                Console.WriteLine("Don't have this subject name, write else");
            }
        }

        private void WriteAllSubjects(List<Subject> subjects)
        {
            Console.WriteLine("Subject names");
            foreach (var subject in subjects)
            {
                Console.WriteLine($"{subject.Name}");
            }
        }

        private Student SelectStudent(List<Student> students)
        {
            while (true)
            {
                Console.WriteLine("If you want to watch all students, click 'Enter'");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    WriteAllStudents(students);
                }
                var studentId = ReadGuidData("Write a student id");
                foreach (var student in students)
                {
                    if (student.Id == studentId)
                    {
                        return student;
                    }
                }
                Console.WriteLine("Don't have this student id, write else");
            }
        }
        private Guid ReadGuidData(string offer)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(offer);
                    return Guid.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void WriteAllStudents(List<Student> students)
        {
            Console.WriteLine("Id | First name | Last name | Group name");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Id} | {student.FirstName,-20} | {student.LastName,-20} | {student.Group.Name}");
            }
        }
        private int ReadScoreValue()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Write down the student's score");
                    var score = Convert.ToInt32(Console.ReadLine());
                    if (score > 0 && score < 6)
                    {
                        return score;
                    }
                    else
                    {
                        Console.WriteLine("Bad input, try again");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public List<Score> ReadScoresToChange(List<Score> scores)
        {
            var newScores = new List<Score>();
            var students = scores.Select(score => score.Student).Distinct().ToList();
            while (true)
            {
                //newScores.Add(SelectScore(GetStudentScores(SelectStudent(students), scores)));
                var student = SelectStudent(students);
                var studentScores = GetStudentScores(student, scores);
                var score = SelectScore(studentScores);
                score.Value = ReadScoreValue();
                newScores.Add(score);
                if (!CheckNeedAddMore("If you want to change more scores, click 'Enter'"))
                {
                    return newScores;
                }
            }
        }
        private List<Score> GetStudentScores(Student student, List<Score> scores)
        {
            return scores.Where(score => score.StudentId == student.Id).ToList();
        }
        private Score SelectScore(List<Score> scores)
        {
            while (true)
            {
                Console.WriteLine("If you want to watch all scores, click");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    WriteAllScores(scores);
                }
                var scoreId = ReadGuidData("Write a score id");
                foreach (var score in scores)
                {
                    if (score.Id == scoreId)
                    {
                        return score;
                    }
                }
            }
        }
        private void WriteAllScores(List<Score> scores)
        {
            Console.WriteLine("Id | score | subject | course");
            foreach (var score in scores)
            {
                Console.WriteLine($"{score.Id,-20} | {score.Value,-20} | {score.Subject.Name,-20} | {score.Course.Name}");
            }
        }

        public ManipulationDataMode ReadMode()
        {
            WriteInstruction();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control)
                {
                    return ManipulationDataMode.AddScores;
                }
                else if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Shift)
                {
                    return ManipulationDataMode.ChangeScores;
                }
                else if (key.Key == ConsoleKey.S)
                {
                    return ManipulationDataMode.AddStudent;
                }
                else if (key.Key == ConsoleKey.D)
                {
                    return ManipulationDataMode.ShowData;
                }
                else if (key.Key == ConsoleKey.I)
                {
                    return ManipulationDataMode.Initialize;
                }
                else if (key.Key == ConsoleKey.R)
                {
                    return ManipulationDataMode.RandomString;
                }
                else if (key.Key == ConsoleKey.U)
                {
                    return ManipulationDataMode.UpdateData;
                }
                else
                {
                    Console.WriteLine("Bad input, try again");
                }
            }
        }
        private void WriteInstruction()
        {
            Console.WriteLine("If you want to add new scores, click 'CTRL + s'");
            Console.WriteLine("If you want to change scores, click 'Shift + s'");
            Console.WriteLine("If you want to add new students, click 's'");
            Console.WriteLine("If you want to watch some data, click 'd'");
            Console.WriteLine("If you want to initialize data, click 'i'");
            Console.WriteLine("If you want to take a random string, click 'r'");
            Console.WriteLine("If you want to update data, click 'u'");
        }

        public DataType SelectDataType()
        {
            Console.WriteLine("If you want to watch students, write 'students'");
            Console.WriteLine("If you want to watch scores, write 'scores'");
            Console.WriteLine("If you want to watch groups, write 'groups'");
            Console.WriteLine("If you want to watch courses, write 'courses'");
            Console.WriteLine("If you want to watch subjects, write 'subjects'");
            Console.WriteLine("If you want to watch specialties, write 'specialties'");
            Console.WriteLine("If you want to watch clever students, write 'clever students'");
            Console.WriteLine("If you want to watch a student scores count, write 'student scores count'");
            Console.WriteLine("If you want to watch a number of scores on courses, write 'number courses scores'");
            while (true)
            {
                var line = Console.ReadLine();
                switch (line)
                {
                    case "students":
                        return DataType.Student;
                    case "scores":
                        return DataType.Score;
                    case "groups":
                        return DataType.Group;
                    case "courses":
                        return DataType.Course;
                    case "subjects":
                        return DataType.Subject;
                    case "specialties":
                        return DataType.Specialty;
                    case "clever students":
                        return DataType.CleverStudents;
                    case "student scores count":
                        return DataType.StudentScoresCount;
                    case "number courses scores":
                        return DataType.NumberCoursesScores;
                    default:
                        Console.WriteLine("Bad input, try again");
                        break;
                }
            }
        }
        private int ReadPageNumber(int count)
        {
            if (count > 1)
            {
                while (true)
                {
                    Console.WriteLine($"Have {count} pages");
                    Console.WriteLine("Write a page number");
                    try
                    {
                        var pageNumber = Convert.ToInt32(Console.ReadLine());
                        if (pageNumber > 0 && pageNumber <= count)
                        {
                            return pageNumber;
                        }
                        else
                        {
                            Console.WriteLine("Bad input, try again");
                        }

                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                return count;
            }
        }
        public void ShowData<TEntity>(IQueryable<TEntity> source)
        {
            if (source == null || source.Count() == 0)
            {
                Console.WriteLine("Don't have data");
            }
            else
            {
                var pageCount = source.Count() / 20;
                var columnsInformationames = CreateColumnsInformation<TEntity>();
                var sortInformation = ReadNeedSortInformation(columnsInformationames.Columns.ToList());
                var pageNumber = ReadPageNumber(pageCount);
                while (true)
                {
                    var needData = OrderBy(source, sortInformation).Skip(pageNumber * 20).Take(20);
                    Console.WriteLine(needData.Count());
                    ShowColumns(columnsInformationames.Columns.Select(column => column.Name).ToArray());
                    columnsInformationames.Action.Invoke(needData);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnsInformationames.Columns.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(pageCount);
                    }
                }
            }
        }
        private void ShowSomeStudents<TEntity>(IQueryable<TEntity> students)
        {
            foreach (var studentEn in students)
            {
                var student = (Student)Convert.ChangeType(studentEn, typeof(TEntity));
                Console.WriteLine($"{student.FirstName,-20} | {student.LastName,-20} | {student.AverageScore,-20} | {student.Group.Name}");
            }
        }
        //public void ShowStudents(IQueryable<Student> students)
        //{
        //    if (students == null || students.Count() == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "First name", "Last name", "Average score", "Group name" };
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(students.Count() / 20);
        //        while (true)
        //        {
        //            var needStudents = OrderBy(students, sortInformation/*, pageNumber*/);
        //            ShowColumns(columnNames);
        //            ShowSomeStudents(needStudents);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(students.Count() / 20);
        //            }
        //        }
        //    }
        //}
        private bool CheckNeedContinue(string offer)
        {
            Console.WriteLine(offer);
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return true;
            }
            return false;
        }
        private void ShowColumns(string[] columnNames)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                Console.Write($"{columnNames[i],-20}");
                if (i < columnNames.Length - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
        private IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, List<Sort> sorts)
        {
            if (sorts.Count() > 0)
            {
                source = OrderBy(source, sorts[0].Field, sorts[0].IsDescending ? "OrderByDescending" : "OrderBy");
                for (int i = 1; i < sorts.Count; i++)
                {
                    source = OrderBy(source, sorts[i].Field, sorts[i].IsDescending ? "ThenByDescending" : "ThenBy");
                }
                return source;
            }
            else
            {
                return source;
            }
        }
        class ColumnsInformation<TEntity>
        {
            public ColumnsInformation()
            {
            }
            public ColumnsInformation(Column[] columns, Action<IQueryable<TEntity>> action)
            {
                Columns = columns;
                Action = action;
            }
            public Column[] Columns { get; set; }
            public Action<IQueryable<TEntity>> Action { get; set; }
        }
        private ColumnsInformation<TEntity> CreateColumnsInformation<TEntity>()
        {
            var entityType = typeof(TEntity);
            if (entityType == typeof(Student))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("First name", CreateField(entityType, new string[]{ "FirstName" })),
                    new Column("Last name", CreateField(entityType, new string[]{ "LastName" })),
                    new Column("Average score", CreateField(entityType, new string[]{ "AverageScore" })),
                    new Column("Group name", CreateField(entityType, new string[]{ "Group", "Name" }))
                },
                    (source) => ShowSomeStudents(source));
            }
            else if (entityType == typeof(Score))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Score", CreateField(entityType, new string[]{ "Value" })),
                    new Column("First name", CreateField(entityType, new string[]{ "Student", "FirstName" })),
                    new Column("Last name", CreateField(entityType, new string[]{ "Student", "LastName" })),
                    new Column("Group name", CreateField(entityType, new string[]{ "Student", "Group", "Name" })),
                    new Column("Subject name", CreateField(entityType, new string[]{ "Subject", "Name" })),
                    new Column("Course name", CreateField(entityType, new string[]{ "Course", "Name" }))
                },
                    (source) => ShowSomeScores(source));
            }
            else if (entityType == typeof(Group))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Group name", CreateField(entityType, new string[]{ "Name" })),
                    new Column("Average score", CreateField(entityType, new string[]{ "AverageScore" })),
                    new Column("Course name", CreateField(entityType, new string[]{ "Course", "Name" })),
                    new Column("Specialty name", CreateField(entityType, new string[]{ "Specialty", "Name" }))
                },
                    (source) => ShowSomeGroups(source));
            }
            else if (entityType == typeof(Course))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Course name", CreateField(entityType, new string[]{ "Name" }))
                },
                    (source) => ShowSomeCourses(source));
            }
            else if (entityType == typeof(SubjectPrototype))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Subject name", CreateField(entityType, new string[]{ "SubjectName" })),
                    new Column("Course score", CreateField(entityType, new string[]{ "CourseName" })),
                    new Column("Specialty name", CreateField(entityType, new string[]{ "SpecialtyName" }))
                },
                    (source) => ShowSomeSubjects(source));
            }
            else if (entityType == typeof(Specialty))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Specialty name", CreateField(entityType, new string[]{ "Name" }))
                },
                    (source) => ShowSomeSpecialties(source));
            }
            else if (entityType == typeof(StudentScoresCount))
            {
                return new ColumnsInformation<TEntity>(new Column[] { new Column("Id", CreateField(entityType, new string[]{ "Id" })),
                    new Column("Student first name", CreateField(entityType, new string[]{ "FirstName" })),
                    new Column("Student last name", CreateField(entityType, new string[]{ "LastName" })),
                    new Column("Score count", CreateField(entityType, new string[]{ "ScoreCount" }))
                },
                    (source) => ShowStudentScoresCount(source));
            }
            return new ColumnsInformation<TEntity>();
        }
        private Field CreateField(Type type, string[] fieldNames)
        {
            var property = type.GetProperty(fieldNames[0]);
            var parameter = Expression.Parameter(type, "parameter");
            var orderByAccess = Expression.MakeMemberAccess(parameter, property);
            for (var i = 1; i < fieldNames.Length; i++)
            {
                property = property.PropertyType.GetProperty(fieldNames[i]);
                orderByAccess = Expression.MakeMemberAccess(orderByAccess, property);
            }
            return new Field(property.PropertyType, parameter, orderByAccess);
        }
        private IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, Field field, string functionName)
        {
            var type = typeof(TEntity);
            var orderByExpression = Expression.Lambda(field.Access, field.AccessParameter);
            var resultExpression = Expression.Call(typeof(Queryable), functionName, new Type[] { type, field.Type }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        private List<Sort> ReadNeedSortInformation(List<Column> columns)
        {
            if (CheckNeedAddMore("If you want to sort data, click 'Enter'"))
            {
                List<Sort> sorts = new List<Sort>();
                while (true)
                {
                    Column needColumn = null;
                    Sort sort = new Sort();
                    sort.IsDescending = ReadSortType();
                    Console.WriteLine("Write what to sort by?");
                    foreach (var column in columns)
                    {
                        Console.WriteLine(column.Name);
                    }
                    var line = Console.ReadLine();
                    foreach (var column in columns)
                    {
                        if (column.Name == line)
                        {
                            needColumn = column;
                        }
                    }
                    if (needColumn != null)
                    {
                        sort.Field = needColumn.Field;
                        sorts.Add(sort);
                        columns.Remove(needColumn);
                        if (columns.Count == 0)
                        {
                            return sorts;
                        }
                        if (!CheckNeedAddMore("If you want to sort more columns, click 'Enter'"))
                        {
                            return sorts;
                        }
                        Console.WriteLine("Write what to sort by next?");
                    }
                    else
                    {
                        if (!CheckNeedAddMore("Bad input, if you want to sort columns, click 'Enter'"))
                        {
                            return sorts;
                        }
                    }
                }
            }
            else
            {
                return new List<Sort>();
            }
        }
        private bool ReadSortType()
        {
            Console.WriteLine("If you want to sort data in original order, click 'Enter'");
            Console.WriteLine("If you want to sort the data in the opposite order, click else");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return false;
            }
            return true;
        }
        //private void ShowSomeStudents(IQueryable<Student> students)
        //{
        //    foreach (var student in students)
        //    {
        //        Console.WriteLine($"{student.FirstName,-20} | {student.LastName,-20} | {student.AverageScore,-20} | {student.Group.Name}");
        //    }
        //}
        //public void ShowScores(List<Score> scores)
        //{
        //    if (scores == null || scores.Count == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "Score", "Student first name", "Student last name", "Group name", "Subject name", "Course name" };
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(scores.Count / 20);
        //        while (true)
        //        {
        //            var needScores = FindNeedScores(scores, sortInformation, pageNumber);
        //            ShowColumns(columnNames);
        //            ShowSomeScores(needScores);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(scores.Count / 20);
        //            }
        //        }
        //    }
        //}


        //public void ShowGroups(List<Group> groups)
        //{
        //    if (groups == null || groups.Count == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "Group name", "Average score", "Course name", "Specialty name" };
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(groups.Count / 20);
        //        while (true)
        //        {
        //            var needGroups = FindNeedGroups(groups, sortInformation, pageNumber);
        //            ShowColumns(columnNames);
        //            ShowSomeGroups(needGroups);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(groups.Count / 20);
        //            }
        //        }
        //    }
        //}

        //public void ShowCourses(List<Course> courses)
        //{
        //    if (courses == null || courses.Count == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "Course name" };
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(courses.Count / 20);
        //        while (true)
        //        {
        //            var needCourses = FindNeedCourses(courses, sortInformation, pageNumber);
        //            ShowColumns(columnNames);
        //            ShowSomeCourses(needCourses);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(courses.Count / 20);
        //            }
        //        }
        //    }
        //}


        //public void ShowSubjects(List<Subject> subjects)
        //{
        //    if (subjects == null || subjects.Count == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "Subject name", "Course name", "Specialty name"};
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(subjects.Count / 20);
        //        var subjectPrototypes = subjects.SelectMany(subject => subject.SubjectCourses.Join(subject.SubjectSpecialties,
        //                subCo => subCo.Subject,
        //                subSpec => subSpec.Subject,
        //                (subCo, subspec) => new SubjectPrototype (subCo.Subject.Name, subCo.Course.Name, subspec.Specialty.Name ))).ToList();
        //        while (true)
        //        {
        //            var needSubjects = FindNeedSubjects(subjectPrototypes, sortInformation, pageNumber);
        //            ShowColumns(columnNames);
        //            ShowSomeSubjects(needSubjects);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(subjects.Count / 20);
        //            }
        //        }
        //    }
        //}
        //private List<SubjectPrototype> FindNeedSubjects(List<SubjectPrototype> subjects, Sort sortInformation, int pageNumber)
        //{
        //    if (sortInformation.sortType == SortType.InOriginalOrder)
        //    {
        //        switch (sortInformation.SortColumns.Count)
        //        {
        //            case 0:
        //                return subjects.Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 1:
        //                return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 2:
        //                return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
        //                    .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 3:
        //                return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
        //                    .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1]))
        //                    .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //        }
        //    }
        //    else if(sortInformation.sortType == SortType.InOppositeOrder)
        //    {
        //        switch (sortInformation.SortColumns.Count)
        //        {
        //            case 0:
        //                return subjects.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 1:
        //                return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 2:
        //                return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
        //                    .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //            case 3:
        //                return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
        //                    .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1]))
        //                    .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
        //        }
        //    }
        //    return subjects;
        //}

        //private object GetSortSubjectColumn(SubjectPrototype subject, string needColumn)
        //{
        //    switch (needColumn)
        //    {
        //        case "Subject name":
        //            return subject.SubjectName;
        //        case "Course name":
        //            return subject.CourseName;
        //        case "Specialty name":
        //            return subject.SpecialtyName;
        //        default:
        //            return "";
        //    }
        //}

        //public void ShowSpecialties(List<Specialty> specialties)
        //{
        //    if (specialties == null || specialties.Count == 0)
        //    {
        //        Console.WriteLine("Don't have students");
        //    }
        //    else
        //    {
        //        var columnNames = new string[] { "Specialty name" };
        //        var sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //        var pageNumber = ReadPageNumber(specialties.Count / 20);
        //        while (true)
        //        {
        //            var needSpecialties = FindNeedSpecialties(specialties, sortInformation, pageNumber);
        //            ShowColumns(columnNames);
        //            ShowSomeSpecialties(needSpecialties);
        //            if (CheckNeedContinue("If you want to exit, click 'Enter'"))
        //            {
        //                return;
        //            }
        //            if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
        //            {
        //                sortInformation = ReadNeedSortInformation(columnNames.ToList());
        //            }
        //            if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
        //            {
        //                pageNumber = ReadPageNumber(specialties.Count / 20);
        //            }
        //        }
        //    }
        //}

        private void ShowSomeScores<TEntity>(IQueryable<TEntity> scores)
        {
            foreach (var entityScore in scores)
            {
                var score = (Score)Convert.ChangeType(entityScore, typeof(TEntity));
                Console.WriteLine($"{score.Value,-20} | {score.Student.FirstName,-20} | {score.Student.LastName,-20} | {score.Student.Group.Name}" +
                    $" | {score.Subject.Name,-20} | {score.Course.Name}");
            }
        }
        private void ShowSomeGroups<TEntity>(IQueryable<TEntity> groups)
        {
            foreach (var entityGroup in groups)
            {
                var group = (Group)Convert.ChangeType(entityGroup, typeof(TEntity));
                Console.WriteLine($"{group.Name,-20} | {group.AverageScore,-20} | {group.Course.Name,-20} | {group.Specialty,-20}");
            }
        }
        private void ShowSomeCourses<TEntity>(IQueryable<TEntity> courses)
        {
            foreach (var entityCourse in courses)
            {
                var course = (Course)Convert.ChangeType(entityCourse, typeof(TEntity));
                Console.WriteLine(course.Name);
            }
        }
        private void ShowSomeSubjects<TEntity>(IQueryable<TEntity> subjects)
        {
            foreach (var entitSubject in subjects)
            {
                var subject = (SubjectPrototype)Convert.ChangeType(entitSubject, typeof(TEntity));
                Console.WriteLine($"{subject.SubjectName,-20} | {subject.CourseName,-20} | {subject.SpecialtyName}");
            }
        }
        private void ShowSomeSpecialties<TEntity>(IQueryable<TEntity> specialties)
        {
            foreach (var entitySpecialty in specialties)
            {
                var specialty = (Specialty)Convert.ChangeType(entitySpecialty, typeof(TEntity));
                Console.WriteLine(specialty.Name);
            }
        }







        public void ShowStudentScoresCount<TEntity>(IQueryable<TEntity> studentScoresCounts)
        {
            foreach (var entityStudentScoreCount in studentScoresCounts)
            {
                var studentScoreCount = (StudentScoresCount)Convert.ChangeType(entityStudentScoreCount, typeof(TEntity));
                Console.WriteLine($"{studentScoreCount.Id,-20} | {studentScoreCount.FirstName,-20} | {studentScoreCount.LastName, -20} | {studentScoreCount.ScoreCount}");
            }
        }

        public SqlParameter ReadMaxFoursCount()
        {
            return new SqlParameter("@maxFoursCount", ReadPositiveNumber("Write a maximum of fours count"));
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public SqlParameter[] ReadParametersForString()
        {
            var minLength = new SqlParameter("@minLength", ReadPositiveNumber("Write the minimum string length"));
            var maxLength = new SqlParameter("@maxLength", ReadPositiveNumber("Write the maximum string length"));
            var chars = new SqlParameter("@chars", ReadChars("Write the characters in the string"));
            var randomString = new SqlParameter() { ParameterName = "@randomString", Size = Convert.ToInt32(@maxLength.Value), Direction = System.Data.ParameterDirection.Output };
            return new SqlParameter[] { minLength, maxLength, chars, randomString };
        }
        private string ReadChars(string offer)
        {
            while (true)
            {
                Console.WriteLine(offer);
                var chars = Console.ReadLine();
                if (chars.Length > 0)
                {
                    return chars;
                }
                else
                {
                    Console.WriteLine("Characters must be greater than 0");
                }
            }
        }
        private int ReadPositiveNumber(string offer)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(offer);
                    var number = Convert.ToInt32(Console.ReadLine());
                    if (number >= 0)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine("The number must be greater than or equal to 0");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public string CreateSql()
        {
            Console.WriteLine("Write the sql command");
            return Console.ReadLine();
        }

        public void WriteRowsNumberAffected(int rowsNumberAffected)
        {
            Console.WriteLine($"{rowsNumberAffected} row(s) affected");
        }

        public void ShowNumberCoursesScores(List<NumberCourseScores> numberCoursesScores)
        {
            var columns = new string[] { "Course name", "Count scores" };
            ShowColumns(columns);
            foreach (var numberCourseScores in numberCoursesScores)
            {
                Console.WriteLine($"{numberCourseScores.CourseName,-20} | {numberCourseScores.Count}");
            }
        }
    }
}
