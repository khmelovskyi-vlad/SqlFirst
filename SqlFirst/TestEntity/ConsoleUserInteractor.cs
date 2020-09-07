using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Score> ReadScoresToChange(List<Score> scores, List<Student> students)
        {
            List<Score> newScores = new List<Score>();
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

        public Mode ReadMode()
        {
            WriteInstruction();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control)
                {
                    return Mode.AddScore;
                }
                else if (key.Key == ConsoleKey.S)
                {
                    return Mode.AddStudent;
                }
                else if (key.Key == ConsoleKey.D)
                {
                    return Mode.ShowData;
                }
                else if (key.Key == ConsoleKey.I)
                {
                    return Mode.Initialize;
                }
                else if (key.Key == ConsoleKey.P)
                {
                    return Mode.Procedure;
                }
                else if (key.Key == ConsoleKey.C && key.Modifiers == ConsoleModifiers.Control)
                {
                    return Mode.ShowCleverStudent;
                }
                else if (key.Key == ConsoleKey.C)
                {
                    return Mode.ShowStudentScoresCount;
                }
                else
                {
                    Console.WriteLine("Bad input, try again");
                }
            }
        }
        private void WriteInstruction()
        {
            Console.WriteLine("If you want to add a new score, click 'CTRL + s'");
            Console.WriteLine("If you want to add a new student, click 's'");
            Console.WriteLine("If you want to watch some data, click 'd'");
            Console.WriteLine("If you want to initialize data, click 'i'");
            Console.WriteLine("If you want to start a procedure, click 'p'");
            Console.WriteLine("If you want to watch clever student, click 'c'");
            Console.WriteLine("If you want to watch a student scores count, click 'CTRL + c'");
        }

        public DataType SelectDataType()
        {
            Console.WriteLine("If you want to watch students, write 'students'");
            Console.WriteLine("If you want to watch scores, write 'scores'");
            Console.WriteLine("If you want to watch groups, write 'groups'");
            Console.WriteLine("If you want to watch courses, write 'courses'");
            Console.WriteLine("If you want to watch subjects, write 'subjects'");
            Console.WriteLine("If you want to watch specialties, write 'specialties'");
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
                        if (pageNumber > 0 && pageNumber < count)
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

        public void ShowStudents(List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                while (true)
                {
                    var pageNumber = ReadPageNumber(students.Count / 20);
                }
            }
        }
        private void ShowSomeStudents(List<Student> students, int pageNumber)
        {
            var firstStudent = students[0];
            var needStudents = students.Take(20).OrderBy(student => "");
            var columnNames = new string[] { nameof(firstStudent.FirstName), nameof(firstStudent.LastName), nameof(firstStudent.AverageScore), nameof(firstStudent.Group.Name) };
            for (int i = 0; i < columnNames.Length; i++)
            {
                Console.Write($"{columnNames,-20}");
                if (i < columnNames.Length - 1)
                {
                    Console.Write("|");
                }
            }
            Console.WriteLine();

            foreach (var student in needStudents)
            {
                Console.WriteLine($"{student.FirstName,-20} | {student.LastName,-20} | {student.AverageScore,-20} | {student.Group.Name}");
            }
        }
        private string ReadNeedSort(string[] columnNames)
        {
            while (true)
            {
                Console.WriteLine("Write what to sort by?");
                foreach (var columnName in columnNames)
                {
                    Console.WriteLine(columnName);
                }
                var line = Console.ReadLine();
                foreach (var columnName in columnNames)
                {
                    if (columnName == line)
                    {
                        return line;
                    }
                }
                Console.WriteLine("Bad input, try again");
            }
        }
        public void ShowScores(List<Score> scores)
        {
            while (true)
            {
                var pageNumber = ReadPageNumber(scores.Count / 20);
            }
        }

        public void ShowGroups(List<Group> groups)
        {
            while (true)
            {
                var pageNumber = ReadPageNumber(groups.Count / 20);
            }
        }

        public void ShowCourses(List<Course> courses)
        {
            while (true)
            {
                var pageNumber = ReadPageNumber(courses.Count / 20);
            }
        }

        public void ShowSubjects(List<Subject> subjects)
        {
            while (true)
            {
                var pageNumber = ReadPageNumber(subjects.Count / 20);
            }
        }

        public void ShowSpecialties(List<Specialty> specialties)
        {
            while (true)
            {
                var pageNumber = ReadPageNumber(specialties.Count / 20);
            }
        }
    }
}
