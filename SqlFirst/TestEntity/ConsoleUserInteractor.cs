using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public Mode ReadMode()
        {
            WriteInstruction();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control)
                {
                    return Mode.AddScores;
                }
                else if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Shift)
                {
                    return Mode.ChangeScores;
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
                else if (key.Key == ConsoleKey.R)
                {
                    return Mode.RandomString;
                }
                else if (key.Key == ConsoleKey.U)
                {
                    return Mode.UpdateData;
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
                var columnNames = new string[] { "First name", "Last name", "Average score", "Group name" };
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(students.Count / 20);
                while (true)
                {
                    var needStudents = FindNeedStudents(students, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeStudents(needStudents);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(students.Count / 20);
                    }
                }
            }
        }
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
        private void ShowSomeStudents(List<Student> students)
        {
            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName,-20} | {student.LastName,-20} | {student.AverageScore,-20} | {student.Group.Name}");
            }
        }
        private List<Student> FindNeedStudents(List<Student> students, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return students.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return students.OrderBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return students.OrderBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return students.OrderBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[1]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return students.OrderBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[1]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[2]))
                            .ThenBy(student => GetSortStudentColumn(student, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else if (sortInformation.sortType == SortType.InOppositeOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return students.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return students.OrderByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return students.OrderByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return students.OrderByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[1]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return students.OrderByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[0]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[1]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[2]))
                            .ThenByDescending(student => GetSortStudentColumn(student, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return students;
        }
        private object GetSortStudentColumn(Student student, string needColumn)
        {
            switch (needColumn)
            {
                case "First name":
                    return student.FirstName;
                case "Last name":
                    return student.LastName;
                case "Average score":
                    return student.AverageScore;
                case "Group name":
                    return student.Group.Name;
                default:
                    return student.Id;
            }
        }
        private SortInformation ReadNeedSortInformation(List<string> columnNames)
        {
            if (CheckNeedAddMore("If you want to sort data, click 'Enter'"))
            {
                SortInformation sortInformation = new SortInformation();
                sortInformation.sortType = ReadSortType();
                Console.WriteLine("Write what to sort by?");
                while (true)
                {
                    var findName = false;
                    foreach (var columnName in columnNames)
                    {
                        Console.WriteLine(columnName);
                    }
                    var line = Console.ReadLine();
                    foreach (var columnName in columnNames)
                    {
                        if (columnName == line)
                        {
                            findName = true;
                        }
                    }
                    if (findName)
                    {
                        sortInformation.SortColumns.Add(line);
                        columnNames.Remove(line);
                        if (columnNames.Count == 0)
                        {
                            return sortInformation;
                        }
                        if (!CheckNeedAddMore("If you want to sort more columns, click 'Enter'"))
                        {
                            return sortInformation;
                        }
                        Console.WriteLine("Write what to sort by next?");
                    }
                    else
                    {
                        if (!CheckNeedAddMore("Bad input, if you want to sort columns, click 'Enter'"))
                        {
                            return sortInformation;
                        }
                    }
                }
            }
            else
            {
                return new SortInformation();
            }
        }
        private SortType ReadSortType()
        {
            Console.WriteLine("If you want to sort data in original order, click 'Enter'");
            Console.WriteLine("If you want to sort the data in the opposite order, click else");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return SortType.InOriginalOrder;
            }
            return SortType.InOppositeOrder;
        }
        public void ShowScores(List<Score> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                var columnNames = new string[] { "Score", "Student first name", "Student last name", "Group name", "Subject name", "Course name" };
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(scores.Count / 20);
                while (true)
                {
                    var needScores = FindNeedScores(scores, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeScores(needScores);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(scores.Count / 20);
                    }
                }
            }
        }

        private void ShowSomeScores(List<Score> scores)
        {
            foreach (var score in scores)
            {
                Console.WriteLine($"{score.Value,-20} | {score.Student.FirstName,-20} | {score.Student.LastName,-20} | {score.Student.Group.Name}" +
                    $" | {score.Subject.Name,-20} | {score.Course.Name}");
            }
        }

        private List<Score> FindNeedScores(List<Score> scores, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return scores.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 5:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[3]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[4])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 6:
                        return scores.OrderBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[3]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[4]))
                            .ThenBy(score => GetSortScoreColumn(score, sortInformation.SortColumns[5])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return scores.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenByDescending(group => GetSortScoreColumn(group, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 5:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[3]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[4])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 6:
                        return scores.OrderByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[0]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[1]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[2]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[3]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[4]))
                            .ThenByDescending(score => GetSortScoreColumn(score, sortInformation.SortColumns[5])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return scores;
        }

        private object GetSortScoreColumn(Score score, string needColumn)
        {
            switch (needColumn)
            {
                case "Score":
                    return score.Value;
                case "Student first name":
                    return score.Student.FirstName;
                case "Student last name":
                    return score.Student.LastName;
                case "Group name":
                    return score.Student.Group.Name;
                case "Subject name":
                    return score.Subject.Name;
                case "Course name":
                    return score.Course.Name;
                default:
                    return "";
            }
        }

        public void ShowGroups(List<Group> groups)
        {
            if (groups == null || groups.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                var columnNames = new string[] { "Group name", "Average score", "Course name", "Specialty name" };
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(groups.Count / 20);
                while (true)
                {
                    var needGroups = FindNeedGroups(groups, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeGroups(needGroups);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(groups.Count / 20);
                    }
                }
            }
        }

        private List<Group> FindNeedGroups(List<Group> groups, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return groups.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return groups.OrderBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return groups.OrderBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return groups.OrderBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[1]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return groups.OrderBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[1]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[2]))
                            .ThenBy(group => GetSortGroupColumn(group, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return groups.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return groups.OrderByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return groups.OrderByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return groups.OrderByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[1]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 4:
                        return groups.OrderByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[0]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[1]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[2]))
                            .ThenByDescending(group => GetSortGroupColumn(group, sortInformation.SortColumns[3])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return groups;
        }

        private object GetSortGroupColumn(Group group, string needColumn)
        {
            switch (needColumn)
            {
                case "Group name":
                    return group.Name;
                case "Average score":
                    return group.AverageScore;
                case "Course name":
                    return group.Course.Name;
                case "Specialty name":
                    return group.Specialty.Name;
                default:
                    return "";
            }
        }
        private void ShowSomeGroups(List<Group> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Name,-20} | {group.AverageScore,-20} | {group.Course.Name,-20} | {group.Specialty,-20}");
            }
        }

        public void ShowCourses(List<Course> courses)
        {
            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                var columnNames = new string[] { "Course name" };
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(courses.Count / 20);
                while (true)
                {
                    var needCourses = FindNeedCourses(courses, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeCourses(needCourses);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(courses.Count / 20);
                    }
                }
            }
        }

        private void ShowSomeCourses(List<Course> courses)
        {
            foreach (var course in courses)
            {
                Console.WriteLine(course.Name);
            }
        }

        private List<Course> FindNeedCourses(List<Course> courses, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return courses.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return courses.OrderBy(specialty => specialty.Name).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return courses.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return courses.OrderByDescending(specialty => specialty.Name).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return courses;
        }

        public void ShowSubjects(List<Subject> subjects)
        {
            if (subjects == null || subjects.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                var columnNames = new string[] { "Subject name", "Course name", "Specialty name"};
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(subjects.Count / 20);
                var subjectPrototypes = subjects.SelectMany(subject => subject.SubjectCourses.Join(subject.SubjectSpecialties,
                        subCo => subCo.Subject,
                        subSpec => subSpec.Subject,
                        (subCo, subspec) => new SubjectPrototype (subCo.Subject.Name, subCo.Course.Name, subspec.Specialty.Name ))).ToList();
                while (true)
                {
                    var needSubjects = FindNeedSubjects(subjectPrototypes, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeSubjects(needSubjects);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(subjects.Count / 20);
                    }
                }
            }
        }
        class SubjectPrototype
        {
            public SubjectPrototype(string subjectName, int courseName, string specialtyName)
            {
                SubjectName = subjectName;
                CourseName = courseName;
                SpecialtyName = specialtyName;
            }
            public string SubjectName { get; set; }
            public int CourseName { get; set; }
            public string SpecialtyName { get; set; }
        }
        private List<SubjectPrototype> FindNeedSubjects(List<SubjectPrototype> subjects, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return subjects.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
                            .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return subjects.OrderBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
                            .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1]))
                            .ThenBy(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else if(sortInformation.sortType == SortType.InOppositeOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return subjects.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 2:
                        return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
                            .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 3:
                        return subjects.OrderByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[0]))
                            .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[1]))
                            .ThenByDescending(subject => GetSortSubjectColumn(subject, sortInformation.SortColumns[2])).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return subjects;
        }

        private object GetSortSubjectColumn(SubjectPrototype subject, string needColumn)
        {
            switch (needColumn)
            {
                case "Subject name":
                    return subject.SubjectName;
                case "Course name":
                    return subject.CourseName;
                case "Specialty name":
                    return subject.SpecialtyName;
                default:
                    return "";
            }
        }
        private void ShowSomeSubjects(List<SubjectPrototype> subjects)
        {
            foreach (var subject in subjects)
            {
                Console.WriteLine($"{subject.SubjectName,-20} | {subject.CourseName,-20} | {subject.SpecialtyName}");
            }
        }

        public void ShowSpecialties(List<Specialty> specialties)
        {
            if (specialties == null || specialties.Count == 0)
            {
                Console.WriteLine("Don't have students");
            }
            else
            {
                var columnNames = new string[] { "Specialty name" };
                var sortInformation = ReadNeedSortInformation(columnNames.ToList());
                var pageNumber = ReadPageNumber(specialties.Count / 20);
                while (true)
                {
                    var needSpecialties = FindNeedSpecialties(specialties, sortInformation, pageNumber);
                    ShowColumns(columnNames);
                    ShowSomeSpecialties(needSpecialties);
                    if (CheckNeedContinue("If you want to exit, click 'Enter'"))
                    {
                        return;
                    }
                    if (CheckNeedContinue("If you want to change sort information, click 'Enter'"))
                    {
                        sortInformation = ReadNeedSortInformation(columnNames.ToList());
                    }
                    if (CheckNeedContinue("If you want to change page number, click 'Enter'"))
                    {
                        pageNumber = ReadPageNumber(specialties.Count / 20);
                    }
                }
            }
        }

        private void ShowSomeSpecialties(List<Specialty> specialties)
        {
            foreach (var specialty in specialties)
            {
                Console.WriteLine(specialty.Name);
            }
        }

        private List<Specialty> FindNeedSpecialties(List<Specialty> specialties, SortInformation sortInformation, int pageNumber)
        {
            if (sortInformation.sortType == SortType.InOriginalOrder)
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return specialties.Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return specialties.OrderBy(specialty => specialty.Name).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            else
            {
                switch (sortInformation.SortColumns.Count)
                {
                    case 0:
                        return specialties.AsEnumerable().Reverse().Skip((pageNumber - 1) * 20).Take(20).ToList();
                    case 1:
                        return specialties.OrderByDescending(specialty => specialty.Name).Skip((pageNumber - 1) * 20).Take(20).ToList();
                }
            }
            return specialties;
        }

        public void ShowStudentScoresCount(List<StudentScoresCount> studentScoresCounts)
        {
            ShowColumns(new string[] { "Id", "Student first name", "Student last name", "Score count"});
            foreach (var studentScoreCount in studentScoresCounts)
            {
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
