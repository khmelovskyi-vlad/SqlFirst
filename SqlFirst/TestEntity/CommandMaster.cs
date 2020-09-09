using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class CommandMaster
    {
        public CommandMaster(IUserInteractor userInteractor, Initializer initializer)
        {
            this.userInteractor = userInteractor;
            this.initializer = initializer;
        }
        private IUserInteractor userInteractor;
        private Initializer initializer;
        public async Task Run()
        {
            while (true)
            {
                var mode = userInteractor.ReadMode();
                switch (mode)
                {
                    case Mode.Initialize:
                        await FirstInitialize();
                        break;
                    case Mode.AddStudent:
                        await AddStudents();
                        break;
                    case Mode.AddScores:
                        await AddScores();
                        break;
                    case Mode.ChangeScores:
                        await ChangeScores();
                        break;
                    case Mode.RandomString:
                        await GetRandomString();
                        break;
                    case Mode.ShowData:
                        await ShowData();
                        break;
                    case Mode.UpdateData:
                        await UpdateData();
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task UpdateData()
        {
            var sql = userInteractor.CreateSql();
            var RowsNumberAffected = await initializer.UpdateData(sql);
            userInteractor.WriteRowsNumberAffected(RowsNumberAffected);
        }
        private async Task GetRandomString()
        {
            var parameters = userInteractor.ReadParametersForString();
            var randomString = await initializer.CreateRandomString(parameters);
            userInteractor.WriteLine(randomString);
        }
        private async Task ChangeScores()
        {
            var allScores = await initializer.GetScores();
            var newScores = userInteractor.ReadScoresToChange(allScores);
            await initializer.ChangeScores(newScores);
        }
        private async Task AddScores()
        {
            var students = await initializer.GetStudents();
            var subjects = await initializer.GetSubjects();//must be GetAllSubjects()?
            var scores = userInteractor.ReadScores(students, subjects);
            await initializer.AddScores(scores);
        }
        private async Task AddStudents()
        {
            var groups = await initializer.GetGroups();
            var newStudents = userInteractor.ReadStudents(groups);
            await initializer.AddStudents(newStudents);
        }
        private async Task FirstInitialize()
        {
            if (await initializer.FirstInitializeData())
            {
                userInteractor.WriteLine("Data was initialized");
            }
            else
            {
                userInteractor.WriteLine("Data wasn't initialized, because it was initially initialized");
            }
        }
        private async Task ShowStudentScoresCount()
        {
            var studentScoresCount = await initializer.GetStudentScoresCount();
            userInteractor.ShowStudentScoresCount(studentScoresCount);
        }
        private async Task ShowCleverStudents()
        {
            var maxFoursCount = userInteractor.ReadMaxFoursCount();
            var cleverStudents = await initializer.GetCleverStudents(maxFoursCount);
            userInteractor.ShowStudents(cleverStudents);
        }
        private async Task ShowNumberCoursesScores()
        {
            var scores = await initializer.GetScores();
            var numberCoursesScores = FillNumberCoursesScores(scores);
            userInteractor.ShowNumberCoursesScores(numberCoursesScores);
        }
        private List<NumberCourseScores> FillNumberCoursesScores(List<Score> scores)
        {
            return scores.GroupBy(
                score => score.Course,
                score => score.Id,
                (key, scoress) => new NumberCourseScores
                (
                    key.Name,
                    scoress.Count()
                )
                )
                .OrderBy(course => course.CourseName)
                .ToList();
        }
        private async Task ShowData()
        {
            var dataType = userInteractor.SelectDataType();
            switch (dataType)
            {
                case DataType.Student:
                    userInteractor.ShowStudents(await initializer.GetStudents());
                    break;
                case DataType.Score:
                    userInteractor.ShowScores(await initializer.GetScores());
                    break;
                case DataType.Group:
                    userInteractor.ShowGroups(await initializer.GetGroups());
                    break;
                case DataType.Course:
                    userInteractor.ShowCourses(await initializer.GetCourses());
                    break;
                case DataType.Subject:
                    userInteractor.ShowSubjects(await initializer.GetSubjects());
                    break;
                case DataType.Specialty:
                    userInteractor.ShowSpecialties(await initializer.GetSpecialties());
                    break;
                case DataType.CleverStudents:
                    await ShowCleverStudents();
                    break;
                case DataType.StudentScoresCount:
                    await ShowStudentScoresCount();
                    break;
                case DataType.NumberCoursesScores:
                    await ShowNumberCoursesScores();
                    break;
                default:
                    break;
            }
        }
    }
}
