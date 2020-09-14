using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class CommandMaster
    {
        public CommandMaster(IUserInteractor userInteractor, Initializer initializer, SqlMaster sqlMaster)
        {
            this.userInteractor = userInteractor;
            this.initializer = initializer;
            this.sqlMaster = sqlMaster;
        }
        private IUserInteractor userInteractor;
        private Initializer initializer;
        private SqlMaster sqlMaster;
        public async Task Run()
        {
            //await sqlMaster.Foo();
            while (true)
            {
                var mode = userInteractor.ReadMode();
                switch (mode)
                {
                    case ManipulationDataMode.AddStudents:
                        await AddStudents();
                        break;
                    case ManipulationDataMode.AddScores:
                        await AddScores();
                        break;
                    case ManipulationDataMode.ChangeScores:
                        await ChangeScores();
                        break;
                    case ManipulationDataMode.RandomString:
                        await GetRandomString();
                        break;
                    case ManipulationDataMode.ShowData:
                        SqlConnector sqlConnector = new SqlConnector();
                        sqlConnector.Connect((universityContext) => ShowData(universityContext));
                        break;
                    case ManipulationDataMode.UpdateData:
                        await UpdateData();
                        break;
                    case ManipulationDataMode.AddStudent:
                        AddStudent();
                        break;
                    default:
                        break;
                }
            }
        }
        private void AddStudent()
        {
            var student = userInteractor.ReadStudent();
            sqlMaster.AddStudent(student);
        }
        private async Task UpdateData()
        {
            var sql = userInteractor.CreateSql();
            var RowsNumberAffected = await sqlMaster.UpdateData(sql);
            userInteractor.WriteRowsNumberAffected(RowsNumberAffected);
        }
        private async Task GetRandomString()
        {
            var parameters = userInteractor.ReadParametersForString();
            var randomString = await sqlMaster.GetRandomString(parameters);
            userInteractor.WriteLine(randomString);
        }
        private async Task ChangeScores()
        {
            var allScores = await sqlMaster.GetScores();
            var newScores = userInteractor.ReadScoresToChange(allScores);
            await sqlMaster.ChangeScores(newScores);
        }
        private async Task AddScores()
        {
            var students = await sqlMaster.GetStudents();
            var subjects = await sqlMaster.GetSubjects();//must be GetAllSubjects()?
            var scores = userInteractor.ReadScores(students, subjects);
            await sqlMaster.AddScores(scores);
        }
        private async Task AddStudents()
        {
            var groups = await sqlMaster.GetGroups();
            var newStudents = userInteractor.ReadStudents(groups);
            await sqlMaster.AddStudents(newStudents);
        }
        private void ShowStudentScoresCount(UniversityContext universityContext)
        {
            var studentScoresCount = universityContext.StudentScoresCounts;
            userInteractor.ShowData(studentScoresCount);
        }
        private void ShowCleverStudents(UniversityContext universityContext)
        {
            var maxFoursCount = userInteractor.ReadMaxFoursCount();
            var cleverStudents = universityContext.Students.FromSqlRaw("SELECT * " +
                    "FROM GetCleverStudents(@maxFoursCount)", maxFoursCount);
            userInteractor.ShowData(cleverStudents);
        }
        private void ShowNumberCoursesScores(UniversityContext universityContext)
        {
            var numberCoursesScores = FillNumberCoursesScores(universityContext);
            userInteractor.ShowNumberCoursesScores(numberCoursesScores);
        }
        private List<NumberCourseScores> FillNumberCoursesScores(UniversityContext universityContext)
        {
            return universityContext.Scores
                    .Include(score => score.Course)
                    .Include(score => score.Subject)
                    .Include(score => score.Student)
                    .ThenInclude(student => student.Group).AsEnumerable().GroupBy(
                score => score.Course,
                score => score.Id,
                (key, scoress) => new NumberCourseScores
                (
                    key.Name,
                    scoress.Count()
                )
                )
                .OrderBy(course => course.CourseName).ToList();
        }
        private void ShowData(UniversityContext universityContext)
        {
            var dataType = userInteractor.SelectDataType();
            switch (dataType)
            {
                case DataType.Student:
                    userInteractor.ShowData(universityContext.Students
                    .Include(student => student.Group));
                    break;
                case DataType.Score:
                    userInteractor.ShowData(universityContext.Scores
                    .Include(score => score.Course)
                    .Include(score => score.Subject)
                    .Include(score => score.Student)
                    .ThenInclude(student => student.Group));
                    break;
                case DataType.Group:
                    userInteractor.ShowData(universityContext.Groups
                    .Include(group => group.Course)
                    .Include(group => group.Specialty));
                    break;
                case DataType.Course:
                    userInteractor.ShowData(universityContext.Courses);
                    break;
                case DataType.Subject:
                    userInteractor.ShowData(universityContext.SubjectPrototypes);
                    break;
                case DataType.Specialty:
                    userInteractor.ShowData(universityContext.Specialties);
                    break;
                case DataType.CleverStudents:
                    ShowCleverStudents(universityContext);
                    break;
                case DataType.StudentScoresCount:
                    ShowStudentScoresCount(universityContext);
                    break;
                case DataType.NumberCoursesScores:
                    ShowNumberCoursesScores(universityContext);
                    break;
                default:
                    break;
            }
        }
    }
}
